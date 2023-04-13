using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Moq;
using Service.Account.DTOs;
using Service.Mappings;
using Service.Services;
using Xunit;
using Service.DTOs.Account;
using Service.SerivceModels;

namespace Test.ServiceTests.Account
{

    public class AccountServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<IUserStore<AppUser>> _mockUserStore;
        public AccountServiceTests()
        {
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new DtoToModel());
                    mc.AddProfile(new ModelToDto());
                });
                IMapper mapper = mapConfig.CreateMapper();
                _mapper = mapper;
            }
            _mockUserStore = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(_mockUserStore.Object, null, null, null, null, null, null, null, null);
        }



        [Fact]
        public async void CreateNewUser_Success()
        {
            //Arrange
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.FindByNameAsync("tuckerishere"));
            RegisterDto registerDto = new RegisterDto()
            {
                UserName = "tuckerishere",
                Password = "Test123!",
                Email = "myTest@email.com"
            };
            var service = new AccountService(_mockUserManager.Object, _mapper);

            //Act
            var result = await service.CreateNewUser(registerDto);

            //Assert
            Assert.True(result.Success);
            Assert.Equal(registerDto.UserName, result.Data.Username);
            Assert.Equal("Successfully created new account.", result.Message);
        }

        [Fact]
        public async void CreateNewUser_UsernameAlreadyExists()
        {
            AppUser appuser = new AppUser()
            {
                UserName = "tuckerishere"
            };
            RegisterDto registerDto = new RegisterDto()
            {
                UserName = "tuckerishere",
                Password = "Test123!",
                Email = "myTest@email.com"
            };
            _mockUserManager.Setup(x => x.FindByNameAsync(registerDto.UserName.ToLower())).ReturnsAsync(appuser);


            var service = new AccountService(_mockUserManager.Object, _mapper);

            //Act
            var result = await service.CreateNewUser(registerDto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("User already exists.", result.Message);
            Assert.Null(result.Data.Username);
        }

        [Fact]
        public async void CreateNewUser_ErrorCreatingName()
        {
            //Arrange
            AppUser appuser = new AppUser()
            {
                UserName = "tuckerishere"
            };
            RegisterDto registerDto = new RegisterDto()
            {
                UserName = "tuckerishere",
                Password = "Test",
                Email = "myTest@email.com"
            };
            IdentityError[] identityErrors = { new IdentityError { Code = "1", Description = "Password error." } };
            _mockUserManager.Setup(x => x.FindByNameAsync(registerDto.UserName.ToUpper()));
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(identityErrors));


            var service = new AccountService(_mockUserManager.Object, _mapper);

            //Act
            var result = await service.CreateNewUser(registerDto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal(identityErrors[0].Description, result.Message);
        }

        [Fact]
        public async void Login_Successful()
        {
            //Arrange
            LoginDto loginDto = new LoginDto()
            {
                Username = "test",
                Password = "Test123!"
            };

            AppUser user = new AppUser()
            {
                UserName = "test"
            };

            UserDto userDto = new UserDto()
            {
                Username = "test"
            };
            ServiceResponse<UserDto> userResponse = new ServiceResponse<UserDto>();
            userResponse.Success = true;
            userResponse.Data = userDto;
            _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username.ToLower())).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(true);

            var service = new AccountService(_mockUserManager.Object, _mapper);
            //Act
            var response = await service.Login(loginDto);

            //Assert
            Assert.True(response.Success);
            Assert.Equal(userDto.Username, response.Data.Username);
            Assert.Equal("Successful Login.", response.Message);
        }

        [Fact]
        public async void Login_InvalidPassword()
        {
            //Arrange
            LoginDto loginDto = new LoginDto()
            {
                Username = "test",
                Password = "Test123!"
            };

            AppUser user = new AppUser()
            {
                UserName = "test"
            };

            UserDto userDto = new UserDto()
            {
                Username = "test"
            };
            ServiceResponse<UserDto> userResponse = new ServiceResponse<UserDto>();
            userResponse.Success = true;
            userResponse.Data = userDto;
            _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username.ToLower())).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(false);

            var service = new AccountService(_mockUserManager.Object, _mapper);
            //Act
            var response = await service.Login(loginDto);

            //Assert
            Assert.False(response.Success);
            Assert.Equal("Invalid Password.", response.Message);
        }

        [Fact]
        public async void Login_InvalidUsername()
        {
            //Arrange
            LoginDto loginDto = new LoginDto()
            {
                Username = "test",
                Password = "Test123!"
            };
            ServiceResponse<UserDto> userResponse = new ServiceResponse<UserDto>();
            userResponse.Success = true;
            _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username.ToLower()));

            var service = new AccountService(_mockUserManager.Object, _mapper);
            //Act
            var response = await service.Login(loginDto);

            //Assert
            Assert.False(response.Success);
            Assert.Equal("Invalid Username.", response.Message);
        }
    }
}
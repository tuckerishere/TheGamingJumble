using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Account.DTOs;
using Service.DTOs.Account;
using Service.SerivceModels;
using Service.Services;

namespace Test.ApiTests.ControllerTests.Account
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountService;
        public AccountControllerTests()
        {
            _accountService = new Mock<IAccountService>();
        }

        [Fact]
        public async Task RegisterUser_Success()
        {
            //Arrange
            UserDto userDto = new UserDto()
            {
                Username = "tuckerishere"
            };
            ServiceResponse<UserDto> serviceResonse = new ServiceResponse<UserDto>()
            {
                Data = userDto,
                Success = true,
                Message = "Successfully created account."
            };
            RegisterDto registerDto = new RegisterDto()
            {
                UserName = "tuckerishere",
                Password = "Test123!",
                Email = "Test@test.com"
            };
            _accountService.Setup(x => x.CreateNewUser(It.IsAny<RegisterDto>())).ReturnsAsync(serviceResonse);

            var controller = new AccountController(_accountService.Object);

            //Act
            var initialResult = await controller.RegisterUser(registerDto);

            //Assert
            var result = (initialResult.Result as CreatedResult).Value as ServiceResponse<UserDto>;
            Assert.Equal(registerDto.UserName, result.Data.Username);
        }

        [Fact]
        public async Task RegisterUser_SuccessStatusCode()
        {
            //Arrange
            UserDto userDto = new UserDto()
            {
                Username = "tuckerishere"
            };
            ServiceResponse<UserDto> serviceResonse = new ServiceResponse<UserDto>()
            {
                Data = userDto,
                Success = true,
                Message = "Successfully created account."
            };
            RegisterDto registerDto = new RegisterDto()
            {
                UserName = "tuckerishere",
                Password = "Test123!",
                Email = "Test@test.com"
            };
            _accountService.Setup(x => x.CreateNewUser(It.IsAny<RegisterDto>())).ReturnsAsync(serviceResonse);

            var controller = new AccountController(_accountService.Object);

            //Act
            var initialResult = await controller.RegisterUser(registerDto);

            //Assert
            var result = (initialResult.Result as CreatedResult).StatusCode;
            Assert.Equal(201, result);
        }

        [Fact]
        public async Task RegisterUser_Error()
        {
            //Arrange
            UserDto userDto = new UserDto()
            {
                Username = "tuckerishere"
            };
            ServiceResponse<UserDto> serviceResonse = new ServiceResponse<UserDto>()
            {
                Data = userDto,
                Success = false,
                Message = "Error createing account."
            };
            RegisterDto registerDto = new RegisterDto()
            {
                UserName = "tuckerishere",
                Password = "Test123!",
                Email = "Test@test.com"
            };
            _accountService.Setup(x => x.CreateNewUser(It.IsAny<RegisterDto>())).ReturnsAsync(serviceResonse);

            var controller = new AccountController(_accountService.Object);

            //Act
            var initialResult = await controller.RegisterUser(registerDto);

            //Assert
            var result = (initialResult.Result as BadRequestObjectResult).Value;
            Assert.Equal(serviceResonse.Message, result);
        }

        [Fact]
        public async Task RegisterUser_ErrorStatusCode()
        {
            //Arrange
            UserDto userDto = new UserDto()
            {
                Username = "tuckerishere"
            };
            ServiceResponse<UserDto> serviceResonse = new ServiceResponse<UserDto>()
            {
                Data = userDto,
                Success = false,
                Message = "Error createing account."
            };
            RegisterDto registerDto = new RegisterDto()
            {
                UserName = "tuckerishere",
                Password = "Test123!",
                Email = "Test@test.com"
            };
            _accountService.Setup(x => x.CreateNewUser(It.IsAny<RegisterDto>())).ReturnsAsync(serviceResonse);

            var controller = new AccountController(_accountService.Object);

            //Act
            var initialResult = await controller.RegisterUser(registerDto);

            //Assert
            var result = (initialResult.Result as BadRequestObjectResult).StatusCode;
            Assert.Equal(400, result);
        }

        [Fact]
        public async Task RegisterUser_NoUsername_BadRequest()
        {
            //Arrange
            var registerDto = new RegisterDto()
            {
                Password = "Test123!",
                Email = "Email@email.com"
            };
            var controller = new AccountController(_accountService.Object);

            //Act
            var initialResult = await controller.RegisterUser(registerDto);
            //Assert
            var badRequestResult = (initialResult.Result as BadRequestObjectResult).Value;
            Assert.Equal("Missing user information", badRequestResult);
        }

        [Fact]
        public async Task RegisterUser_NoUsername_BadRequestStatusCode()
        {
            //Arrange
            var registerDto = new RegisterDto()
            {
                Password = "Test123!",
                Email = "Email@email.com"
            };
            var controller = new AccountController(_accountService.Object);

            //Act
            var initialResult = await controller.RegisterUser(registerDto);
            //Assert
            var badRequestResult = (initialResult.Result as BadRequestObjectResult).StatusCode;
            Assert.Equal(400, badRequestResult);
        }

        [Fact]
        public async Task RegisterUser_NoPassword_BadRequest()
        {
            //Arrange
            var registerDto = new RegisterDto()
            {
                UserName = "username",
                Email = "Email@email.com"
            };
            var controller = new AccountController(_accountService.Object);

            //Act
            var initialResult = await controller.RegisterUser(registerDto);
            //Assert
            var badRequestResult = (initialResult.Result as BadRequestObjectResult).Value;
            Assert.Equal("Missing user information", badRequestResult);
        }

        [Fact]
        public async Task RegisterUser_NoEmail_BadRequest()
        {
            //Arrange
            var registerDto = new RegisterDto()
            {
                UserName = "username",
                Password = "Test123!"
            };
            var controller = new AccountController(_accountService.Object);

            //Act
            var initialResult = await controller.RegisterUser(registerDto);
            //Assert
            var badRequestResult = (initialResult.Result as BadRequestObjectResult).Value;
            Assert.Equal("Missing user information", badRequestResult);
        }
    }
}
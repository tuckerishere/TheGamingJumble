using System.Text;
using System.Collections.Immutable;
using System.Xml.Linq;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Account.DTOs;
using Service.DTOs.Account;
using Service.SerivceModels;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AccountService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<UserDto>> CreateNewUser(RegisterDto registerDto)
        {
            ServiceResponse<UserDto> response = new ServiceResponse<UserDto>();
            if (await UserExists(registerDto.UserName))
            {
                response = SetServiceResponse(new UserDto(), false, "User already exists.");
            }
            if (response.Success)
            {
                var user = _mapper.Map<RegisterDto, AppUser>(registerDto);
                user.UserName = user.UserName.ToLower();

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    var newUser = new UserDto
                    {
                        Username = registerDto.UserName
                    };
                    response = SetServiceResponse(newUser, true, "Successfully created new account.");
                }
                else
                {
                    StringBuilder errors = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        errors.Append(error.Description);
                    }
                    response = SetServiceResponse(new UserDto(), false, errors.ToString());
                }
            }
            return response;
        }

        public async Task<ServiceResponse<UserDto>> Login(LoginDto loginDto)
        {
            ServiceResponse<UserDto> response = new ServiceResponse<UserDto>();
            var user = await _userManager.FindByNameAsync(loginDto.Username.ToLower());
            if (user != null)
            {
                var password = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (password)
                {
                    var userReturned = _mapper.Map<AppUser, UserDto>(user);
                    response = SetServiceResponse(userReturned, true, "Successful Login.");
                }
                else
                {
                    response = SetServiceResponse(new UserDto(), false, "Invalid Password.");
                }
            }
            else
            {
                response = SetServiceResponse(new UserDto(), false, "Invalid Username.");
            }

            return response;
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _userManager.FindByNameAsync(userName.ToLower()) != null ? true : false;
        }

        private ServiceResponse<UserDto> SetServiceResponse(UserDto user, bool success, string message)
        {
            return new ServiceResponse<UserDto>
            {
                Data = user,
                Success = success,
                Message = message
            };
        }
    }
};
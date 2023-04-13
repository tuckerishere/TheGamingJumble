using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Account.DTOs;
using Service.DTOs.Account;
using Service.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] RegisterDto registerDto)
        {
            if ((string.IsNullOrEmpty(registerDto.UserName)
                || string.IsNullOrEmpty(registerDto.Email)
                || string.IsNullOrEmpty(registerDto.Password)))
            {
                return BadRequest("Missing user information");
            }
            var response = await _accountService.CreateNewUser(registerDto);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return new CreatedResult("Registered User", response.Data);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            if(string.IsNullOrEmpty(loginDto.Username) 
                || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Missing login information.");
            }
            var response = await _accountService.Login(loginDto);
            if(!response.Success)
            {
                return BadRequest(response.Message);
            }

            return response.Data;
        }
    }
}
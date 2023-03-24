using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;
using Service.Account.DTOs;
using Service.DTOs.Account;
using Service.SerivceModels;

namespace Service.Services
{
    public interface IAccountService
    {
        Task<ServiceResponse<UserDto>> CreateNewUser(RegisterDto registerDto);
    }
}
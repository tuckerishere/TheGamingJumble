using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Account.DTOs
{
    public class RegisterDto
    {
        
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
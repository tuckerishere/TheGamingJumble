using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data.Entity
{
    public class AppUser : IdentityUser<int>
    {
        List<AppUserRole>? UserRoles {get;set;}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data.Entity
{
    public class AppRole : IdentityRole<int>
    {
        List<AppUserRole>? UserRoles {get; set; }
    }
}
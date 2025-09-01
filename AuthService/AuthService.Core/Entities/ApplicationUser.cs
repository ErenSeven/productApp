using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; } = "User";
    }
}
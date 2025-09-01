using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Core.Events
{
    public class UserLoggedInEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime LoggedInAt { get; set; }
    }
}
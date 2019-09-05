using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPracticeWebSite.IdentityUserModel
{
    public class MyPracticeUserIdentity:IdentityUser
    {
        public DateTime DOB { get; set; }
    }
}

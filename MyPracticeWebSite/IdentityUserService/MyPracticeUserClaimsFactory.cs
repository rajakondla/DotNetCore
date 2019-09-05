using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyPracticeWebSite.IdentityUserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyPracticeWebSite.IdentityUserService
{
    public class MyPracticeUserClaimsFactory:UserClaimsPrincipalFactory<MyPracticeUserIdentity,IdentityRole>
    {
        public MyPracticeUserClaimsFactory(UserManager<MyPracticeUserIdentity> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options):base(userManager,roleManager,options)
        {

        }

        public override async Task<ClaimsPrincipal> CreateAsync(MyPracticeUserIdentity user)
        {
           var claimsPrincipal=await base.CreateAsync(user);
            var identity = claimsPrincipal.Identities.First();

            identity.AddClaim(new Claim("DOB",user.DOB.ToString("dd/MM/yyyy")));

            return claimsPrincipal;
        }
            
    }
}

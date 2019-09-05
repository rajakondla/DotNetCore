using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPracticeWebSite.IdentityUserModel;
using Shared.Models;

namespace MyPracticeWebSite.IdentityUserService
{
    public class MyPracticeDBContext: IdentityDbContext<MyPracticeUserIdentity>
    {
        public MyPracticeDBContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Order> Order { get; set; }
    }
}

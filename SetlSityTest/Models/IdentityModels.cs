using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SetlSityTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string name) : base(name)
        {
        }
    }

    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("SetlSityTest") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

    }
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            //ApplicationContext db = new ApplicationContext();
            //ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            ApplicationContext db = context.Get<ApplicationContext>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            return manager;
        }
    }

    public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var passwordHasher = new PasswordHasher();
            var user = new ApplicationUser("admin")
            {
                PasswordHash = passwordHasher.HashPassword("123456"),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleOne = new IdentityRole("OneTask");
            context.Roles.Add(roleOne);

            var roleTwo = new IdentityRole("TwoTask");
            context.Roles.Add(roleTwo);

            var roleThree = new IdentityRole("ThreeTask");
            context.Roles.Add(roleThree);

            var roleFour = new IdentityRole("FourTask");
            context.Roles.Add(roleFour);

            var roleFive = new IdentityRole("FiveTask");
            context.Roles.Add(roleFive);

            var roleSix = new IdentityRole("SixTask");
            context.Roles.Add(roleSix);

            var roleSeven = new IdentityRole("SevenTask");
            context.Roles.Add(roleSeven);

            var roleAdmin = new IdentityRole("admin");
            context.Roles.Add(roleAdmin);

            //var role1 = new IdentityUserRole
            //{
            //    RoleId = roleTwo.Id,
            //    UserId = user.Id
            //};

            //var role2 = new IdentityUserRole
            //{
            //    RoleId = roleThree.Id,
            //    UserId = user.Id
            //};

            //var role3 = new IdentityUserRole
            //{
            //    RoleId = roleFour.Id,
            //    UserId = user.Id
            //};

            //var role4 = new IdentityUserRole
            //{
            //    RoleId = roleFive.Id,
            //    UserId = user.Id
            //};

            //var role5 = new IdentityUserRole
            //{
            //    RoleId = roleOne.Id,
            //    UserId = user.Id
            //};

            //var role6 = new IdentityUserRole
            //{
            //    RoleId = roleSix.Id,
            //    UserId = user.Id
            //};

            //var role7 = new IdentityUserRole
            //{
            //    RoleId = roleSeven.Id,
            //    UserId = user.Id
            //};

            var rA = new IdentityUserRole
            {
                RoleId = roleAdmin.Id,
                UserId = user.Id,
            };

            //user.Roles.Add(role1);
            //user.Roles.Add(role2);
            //user.Roles.Add(role3);
            //user.Roles.Add(role4);
            //user.Roles.Add(role5);
            //user.Roles.Add(role6);
            //user.Roles.Add(role7);
            user.Roles.Add(rA);

            context.Users.Add(user);

            base.Seed(context);
        }
    }
}
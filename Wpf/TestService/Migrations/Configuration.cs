namespace TestService.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TestModels;

    internal sealed class Configuration : DbMigrationsConfiguration<TestService.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestService.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            roleManager.Create(new IdentityRole { Name = ApplicationRoles.SuperAdmin });
            roleManager.Create(new IdentityRole { Name = ApplicationRoles.Admin });
            roleManager.Create(new IdentityRole { Name = ApplicationRoles.User });

            if (!context.Users.Select(rec => rec.UserName).Contains("Admin"))
            {
                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);

                var user = new User
                {
                    FIO = "Default",
                    UserName = "Admin",
                    PasswordHash = "Admin777"
                };

                manager.Create(user, user.PasswordHash);
                manager.AddToRole(user.Id, ApplicationRoles.SuperAdmin);

            }
            context.SaveChangesAsync();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}

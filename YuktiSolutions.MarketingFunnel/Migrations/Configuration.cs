namespace YuktiSolutions.MarketingFunnel.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using YuktiSolutions.MarketingFunnel.Models.Database;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (context.Roles.Any() == false)
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                // System Admin it the top most user who will manage the cloud
                manager.Create(new IdentityRole { Name = "Admin" });
                // Manager will manage one particular workspace
                manager.Create(new IdentityRole { Name = "Manager" });
                // Sales executive
                manager.Create(new IdentityRole { Name = "Editor" });
                // Marketing executive
                manager.Create(new IdentityRole { Name = "Content Writer" });
            }
            if (context.Users.Any() == false)
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var rakesh = new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    DisplayName = "Rakesh Verma",
                    Email = "rakesh.verma@yuktisolutions.com",
                    UserName = "rakesh.verma@yuktisolutions.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                if (manager.Create(rakesh, "Winner2020@").Succeeded)
                {
                    manager.AddToRole(rakesh.Id, "Admin");
                }
            }
        }
    }
}

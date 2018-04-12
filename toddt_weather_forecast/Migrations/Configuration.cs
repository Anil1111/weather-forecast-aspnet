namespace toddt_weather_forecast.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using toddt_weather_forecast.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<toddt_weather_forecast.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "toddt_weather_forecast.Models.ApplicationDbContext";
        }

        protected override void Seed(toddt_weather_forecast.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // the below use bad passwords, but since this is just test data I will be doing it this way.
            var passwordHasher = new PasswordHasher();
            var samplePassword = "Asdf1234@";
            var sampleEmailOne = "wptran58@gmail.com";
            var sampleEmailTwo = "wptran59@gmail.com";
            var user = new ApplicationUser
            {
                Email = sampleEmailOne,
                EmailConfirmed = false,
                PasswordHash = passwordHasher.HashPassword(samplePassword),
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                UserName = sampleEmailOne
            };

            var secondUser = new ApplicationUser
            {
                Email = sampleEmailTwo,
                EmailConfirmed = false,
                PasswordHash = passwordHasher.HashPassword(samplePassword),
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                UserName = sampleEmailTwo
            };

            // add to the table.
            context.Users.Add(user);
            context.Users.Add(secondUser);
        }
    }
}

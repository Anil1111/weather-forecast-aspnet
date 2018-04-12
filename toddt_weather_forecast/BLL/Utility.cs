using System.Web;
using toddt_weather_forecast.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.IO;
using System.Web.Routing;

namespace toddt_weather_forecast.BLL
{
    /**
     * TODO: rename this class
     * This class will provide helper functions that are used in different business logic layer classes or that can be 
     * used for automated tests.
     */
    public class Utility
    {
        private ApplicationDbContext _dbContext; // allow access to the database.

        public Utility()
        {
            _dbContext = new ApplicationDbContext();
        }

        /**
         * helper method to check if a user is logged in
         */
        public bool CheckIfUserLoggedIn()
        {
            var isLoggedIn = HttpContext.Current?.User?.Identity?.IsAuthenticated;
            if (isLoggedIn == null) return false;
            return (bool) isLoggedIn;
        }

        /**
         * grabs the ID of a logged in user.
         */
        public int GetIdOfCurrentLoggedInUser()
        {
            var userId = -1;
            if (CheckIfUserLoggedIn())
            {
                userId = HttpContext.Current.User.Identity.GetUserId<int>();
            }
            return userId;
        }

        /**
         * this creates a user into the database with the provided email and password using the e-mail as the user name.
         * TODO: This may need to be moved....
         */
        public void CreateSampleUser(string email, string password)
        {
            var passwordHasher = new PasswordHasher();
            var user = new ApplicationUser
            {
                Email = email,
                EmailConfirmed = false,
                PasswordHash = passwordHasher.HashPassword(password),
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                UserName = email
            };
            _dbContext.Users.Add(user);
        }

        /**
         * This creates an HttpContext mock object for unit testing.
         * TODO: This may need to be moved..
         */ 
        public void SetControllerContext(Controller controller)
        {
            HttpRequest httpRequest = new HttpRequest("", "http://localhost:55412/", "");
            StringWriter stringWriter = new StringWriter();
            HttpResponse httpResponse = new HttpResponse(stringWriter);
            HttpContext httpContextMock = new HttpContext(httpRequest, httpResponse);
            controller.ControllerContext = new ControllerContext(new HttpContextWrapper(httpContextMock), new RouteData(), controller);
        }

    }
}
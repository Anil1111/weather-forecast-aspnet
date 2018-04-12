using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toddt_weather_forecast;
using toddt_weather_forecast.BLL;
using toddt_weather_forecast.Controllers;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        [TestMethod]
        public void Index()
        {
            // query the database.. let's see what it gets...
            /*
            var searches = from search in _dbContext.Searches
                           select search;

            var users = from user in _dbContext.Searches
                        select user;

            int numberOfUsers = users.Count();
            Utility utility = new Utility(); // get access to utility functions.

            utility.CreateSampleUser("wptran58@gmail.com", "Asdf1234@");
            users = from user in _dbContext.Searches
                    select user;
            int numberOfUsersAfterCreation = users.Count();

            Assert.AreEqual(numberOfUsers, numberOfUsersAfterCreation);
            */
            // this is only testing to see if the ViewBag property can be properly accessed and the value retrieved.
            string expectedHomePageName = "Home Page";
            // Arrange
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.AreEqual(expectedHomePageName, result.ViewBag.PageTitleName);

            /*
            AccountController accountController = new AccountController();
            LoginViewModel model = new LoginViewModel
            {
                Email = "wptran58@gmail.com",
                Password = "Asdf1234@",
                RememberMe = false
            };
            var actionResult = await accountController.Login(model, null);

            string s = "";
            */
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);

        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}

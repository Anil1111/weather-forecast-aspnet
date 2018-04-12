using Microsoft.VisualStudio.TestTools.UnitTesting;
using toddt_weather_forecast.Controllers;
using toddt_weather_forecast.Models;
using toddt_weather_forecast.BLL;
using System.Web;
using System;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Security.Principal;
using System.IO;
using System.Linq;

namespace toddt_weather_forecast.Tests.Controllers
{
    [TestClass]
    public class PreviousSearchControllerTest
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private string TestEmail;
        private String EncryptedPassword;
        private String PlainTextPassword;
        private PasswordHasher PasswordHasher;
        private Mock<ApplicationUserManager> _userManager;
        private Mock<ApplicationSignInManager> _signInManager;

        [TestInitialize]
        public void InitializeFields()
        {
            PasswordHasher = new PasswordHasher();
            TestEmail = "wptran100@gmail.com";
            PlainTextPassword = "Asdf1234@";
            EncryptedPassword = PasswordHasher.HashPassword(PlainTextPassword);
            var authenticationManager = new Mock<IAuthenticationManager>();
            var userStore = new Mock<UserStore>(_dbContext);
            _userManager = new Mock<ApplicationUserManager>(userStore.Object);
            _signInManager = new Mock<ApplicationSignInManager>(_userManager.Object, authenticationManager.Object);
        }

        [TestMethod]
        //public async Task ShowPreviousSearchTestAsync()
        public void ShowPreviousSearchTestAsync()
        {
            HttpContext.Current = CreateHttpContext(userLoggedIn: true);
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            AccountController accountController = new AccountController(_userManager.Object, _signInManager.Object);
            accountController.Url = url; // must set HttpContext.

            RegisterViewModel registerViewModel = new RegisterViewModel
            {
                Email = TestEmail,
                Password = PlainTextPassword,
                ConfirmPassword = PlainTextPassword
            };

            LoginViewModel loginViewModel = new LoginViewModel
            {
                Email = TestEmail,
                Password = EncryptedPassword,
                RememberMe = false
            };

            var users = from s in _dbContext.Users
                        select s;
            Assert.AreEqual(0, users.Count());

            //var registerResult = await accountController.Register(registerViewModel);

            //var actionResult = await accountController.Login(loginViewModel, null) as RedirectToRouteResult;
            //Assert.AreEqual("Index", actionResult.RouteValues["action"]);

            PreviousSearchController previousSearchController = new PreviousSearchController();
            ViewResult previousSearch = previousSearchController.ShowPreviousSearch() as ViewResult;
            string viewName = previousSearch.ViewName;
            string expectedErrorMessageWithNoPreviousSearches = "You have not made any searches";

            PreviousSearchPagedList pagedList = (PreviousSearchPagedList) previousSearch.Model;
            Assert.AreEqual(0, pagedList.Count);

            Console.WriteLine();
        }

        [TestMethod]
        //public async Task ShowPreviousSearchTestAsync()
        public void ShowPreviousSearchTest()
        {
            PreviousSearchController controller = new PreviousSearchController();
            // simulate login
            AccountController accountController = new AccountController(_userManager.Object, _signInManager.Object);
            LoginViewModel loginViewModel = new LoginViewModel
            {
                Email = TestEmail,
                Password = EncryptedPassword,
                RememberMe = false
            };
            /*
            var actionResult = await accountController.Login(loginViewModel, "");
            var result = actionResult as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
            */
           var result = accountController.Register();

            Console.WriteLine();

            // visit the controller


            // Assert
            /*
            Assert.IsTrue(AuthorizationTest.IsAuthorized(
                controller,
                "PrevSearch",
                null
                ));
            */

            /*
            Assert.IsTrue(AuthorizationTest.IsAuthorized(
                controller,
                "ShowPreviousSearch",
                new Type[] { typeof(int) }
                ));
            */

            //controller.Url = new UrlHelper();
            //var fullURL = controller.Url.Action("ShowPreviousSearch", "PreviousSearchController", null);
            // Act
            // ViewResult result = controller.ShowPreviousSearch() as ViewResult;
            // Assert
            // Assert.AreEqual("Login", result.ViewName);
        }

        [TestMethod]
        public void GET__Register_UserLoggedIn_RedirectsToHomeIndex()
        {
            // Arrange
            /*
            var userStore = new Mock<UserStore>(_dbContext);
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>();
            var signInManager = new Mock<ApplicationSignInManager>(userManager.Object, authenticationManager.Object);
            var accountController = new AccountController(userManager.Object, signInManager.Object);
            */

            var userStore = new Mock<UserStore>(_dbContext);
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>();
            var signInManager = new Mock<ApplicationSignInManager>(userManager.Object, authenticationManager.Object);
            var accountController = new AccountController(userManager.Object, signInManager.Object);

            // Act
            var result = accountController.Register();

            // Assert
            //Assert.Equals("Register", result.ViewName);
        }

        [TestMethod]
        public void GET__Register_UserLoggedOut_ReturnsView()
        {
            // Arrange
            var userStore = new Mock<UserStore>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>();
            var signInManager = new Mock<ApplicationSignInManager>(userManager.Object, authenticationManager.Object);

            var accountController = new AccountController(
                userManager.Object, signInManager.Object);

            // Act
            var result = accountController.Register();

            // Assert
        }

        // example: https://stackoverflow.com/questions/28405966/how-to-mock-applicationusermanager-from-accountcontroller-in-mvc5
        private static HttpContext CreateHttpContext(bool userLoggedIn)
        {
            var httpContext = new HttpContext(
                new HttpRequest(string.Empty, "http://yahoo.com", string.Empty),
                new HttpResponse(new StringWriter())
            )
            {
                User = userLoggedIn
                    ? new GenericPrincipal(new GenericIdentity("userName"), new string[0])
                    : new GenericPrincipal(new GenericIdentity(string.Empty), new string[0])
            };

            return httpContext;
        }
    }
}

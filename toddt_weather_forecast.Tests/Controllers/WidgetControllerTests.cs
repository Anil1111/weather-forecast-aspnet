using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using toddt_weather_forecast.Controllers;
using toddt_weather_forecast.Tests.Authorization;

namespace toddt_weather_forecast.Tests.Controllers
{
    [TestClass]
    public class WidgetControllerTests
    {
        [TestMethod]
        public void Index_IsAnonymous()
        {
            // Arrange
            WidgetController controller = new WidgetController();

            // Assert
            Assert.IsTrue(AuthorizationTest.IsAnonymous(
                controller,
                "Index",
                null));
        }

        [TestMethod]
        public void Details_IsAuthorized()
        {
            // Arrange
            WidgetController controller = new WidgetController();

            // Asset
            Assert.IsTrue(AuthorizationTest.IsAuthorized(
                controller,
                "Details",
                new Type[] { typeof(int) }));
        }

        [TestMethod]
        public void Create_Get_IsAuthorized()
        {
            // Arrange
            WidgetController controller = new WidgetController();

            // Assert
            Assert.IsTrue(AuthorizationTest.IsAuthorized(
                controller,
                "Create",
                null,
                new string[] { "Admin" },
                new string[] { "Ross" }));
        }

        [TestMethod]
        public void Create_Post_IsAuthorized()
        {
            // Arrange
            WidgetController controller = new WidgetController();

            // Assert
            Assert.IsTrue(AuthorizationTest.IsAuthorized(
                controller,
                "Create",
                new Type[] { typeof(FormCollection) },
                new string[] { "Admin" },
                new string[] { "Ross" }));
        }
    }
}

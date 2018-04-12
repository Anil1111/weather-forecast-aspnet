using System.Web.Mvc;

namespace toddt_weather_forecast.Controllers
{
    /**
     * This controller provides actions responsible for displaying the home page to the user which includes allowing
     * the user to enter in an address to search for daily or hourly forecasts.
     */ 
    public class HomeController : Controller
    {
        private const string HomePageName = "Home Page";
        public ActionResult Index()
        {
            ViewBag.PageTitleName = HomePageName;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
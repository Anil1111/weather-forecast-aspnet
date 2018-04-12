using System.Web.Mvc;
using toddt_weather_forecast.BLL;
using static System.String;

namespace toddt_weather_forecast.Controllers
{
    /**
     * This controller contains functions that holds actions which allows the user to get daily or hourly forecasts.
     */ 
    public class SearchController : Controller
    {
        /**
         * this function allows for a latitude and longitude pair to be passed in as parameters and is required to be 
         * used via a GET request.
         * this can be done if the user happens to know the latitude and longitude. if either are missing the user will 
         * be redirected to the home page.
         * TODO: add in an error message to the home page!
         */ 
        public ActionResult HourlyForecastResults(string latitude, string longitude, int? page)
        {
            if(IsNullOrEmpty(latitude) || IsNullOrEmpty(longitude))
            {
                return RedirectToAction("Index", "Home");
            }

            var retrieveForecast = new RetrieveForecast();
            var pageNumber = page ?? 1;
            var hourlyForecastPagedList = retrieveForecast.GetHourlyForecastsFromGetRequest(latitude, longitude, 
                pageNumber);
            return View(hourlyForecastPagedList);
        }   

        /**
         * this action will respond to when the user enters in an address and will convert that to a latitude and 
         * longitude pair.
         * after the latitude and longitude pair has been computed then this function will return an object which holds 
         * hourly forecasts for a location.
         */ 
        [HttpPost]
        public ActionResult HourlyForecastResults(string enteredAddress)
        {
            var retrieveForecast = new RetrieveForecast();
            var hourlyForecastPagedList = retrieveForecast.GetHourlyForecastsFromPostRequest(enteredAddress);
            return View(hourlyForecastPagedList);
        }

        /**
         * this action responds to when a user has provided a latitude and longitude pair and is attempting to search 
         * for daily forecasts.
         */
        public ActionResult DailyForecastResults(string latitude, string longitude, int? page)
        {
            if (IsNullOrEmpty(latitude) || IsNullOrEmpty(longitude))
            {
                return RedirectToAction("Index", "Home");
            }

            var retrieveForecast = new RetrieveForecast();
            var pageNumber = page ?? 1;
            var dailyForecastPagedList = retrieveForecast.GetDailyForecastsFromGetRequest(latitude, longitude, 
                pageNumber);
            return View(dailyForecastPagedList);
        }


    }
}
using System.Web.Mvc;
using toddt_weather_forecast.BLL;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.Controllers
{
    /**
     * This controller contains functions which assist with displaying previous searches a logged in user has made.
     */
    [Authorize]
    public class PreviousSearchController : Controller
    {
        private const string IndexActionName = "Index";
        private const string HomeControllerName = "Home";
        private const string NotAllowedToViewString = "you cannot view these prior search results";
        private const string ErrorMessageKey = "errorMessage";

        [Authorize]
        public ActionResult PrevSearch()
        {
            return View();
        }

        [Authorize]
        public ActionResult ShowPreviousSearch(int page = 1)
        {
            var parsePreviousSearch = new ParsePreviousSearch();
            PreviousSearchPagedList previousSearchPagedList = parsePreviousSearch.GetPriorSearchesFromLoggedInUser(page);
            return View(previousSearchPagedList);
        }

        /**
         * this action responds to when the user is requesting for hourly forecasts for a prior search or when a user first
         * clicks on the forecasts for a previous search.
         */
        [Authorize]
        public ActionResult HourlyForecastResultsFromSearch(int searchId, int page = 1)
        {
            // TODO: redirect to home with an error message!
            if (searchId <= 0) return RedirectToAction(IndexActionName, HomeControllerName);
            var retrieveForecast = new RetrieveForecast();
            var hourlyForecastPagedListFromSearch = retrieveForecast.GetHourlyForecastsFromPreviousSearch(page, searchId);
            if (hourlyForecastPagedListFromSearch == null)
            {
                TempData[ErrorMessageKey] = NotAllowedToViewString;
                return RedirectToAction(IndexActionName, HomeControllerName);
            }
            return View(hourlyForecastPagedListFromSearch);
        }

        /**
         * this action responds to when the user is requesting for daily forecasts for a prior search.
         */
        [Authorize]
        public ActionResult DailyForecastResultsFromSearch(int searchId, int page = 1)
        {
            // TODO: redirect to home with an error message!
            if (searchId <= 0) return RedirectToAction(IndexActionName, HomeControllerName);
            var retrieveForecast = new RetrieveForecast();
            var dailyForecastPagedListFromSearch = retrieveForecast.GetDailyForecastsFromPreviousSearch(page, searchId);
            if (dailyForecastPagedListFromSearch == null)
            {
                TempData[ErrorMessageKey] = NotAllowedToViewString;
                return RedirectToAction(IndexActionName, HomeControllerName);
            }
            return View(dailyForecastPagedListFromSearch);
        }

    }
}
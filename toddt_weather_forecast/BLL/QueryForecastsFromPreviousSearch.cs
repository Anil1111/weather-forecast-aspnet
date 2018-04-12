using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    /**
     * This class provides functions to query prior daily and hourly forecasts by using an associated search ID.
     */ 
    public class QueryForecastsFromPreviousSearch
    {
        private ApplicationDbContext _dbContext;
        private Utility Utility;

        public QueryForecastsFromPreviousSearch()
        {
            _dbContext = new ApplicationDbContext();
            Utility = new Utility();
        }

        /**
         * A helper function which checks if the currently logged in user made the search. This is done by retrieving
         * the search by the passed in search ID and then grabbing the UserId field of the search to see if it matches
         * the ID of the logged in user.
         */ 
        private bool CheckIfSearchIsMadeByLoggedInUser(int searchId)
        {
            var searchWithId = from s in _dbContext.Searches
                           where s.Id.Equals(searchId)
                           select s;
            List<Search> searchList = searchWithId.ToList();
            Search search = searchList[0]; // grab the first and only Search element.
            int loggedInUserId = Utility.GetIdOfCurrentLoggedInUser();
            if (loggedInUserId != search.UserId) return false;
            return true;
        }

        /**
         * Return all the daily forecasts that are a part of the passed in searchId.
         */ 
        public List<DailyForecast> GetDailyForecastsFromSearchId(int searchId)
        {
            var dailyForecastsFromSearch = new List<DailyForecast>();
            if (!CheckIfSearchIsMadeByLoggedInUser(searchId)) return dailyForecastsFromSearch;
            var dailyForecasts = from df in _dbContext.DailyForecasts
                                 where df.SearchId.Equals(searchId)
                                 select df;
            dailyForecastsFromSearch = dailyForecasts.ToList();
            return dailyForecastsFromSearch;
        }

        /**
         * Return all the hourly forecasts that are a part of the passed in searchId.
         */
        public List<HourlyForecast> GetHourlyForecastsFromSearchId(int searchId)
        {
            var hourlyForecastsFromSearch = new List<HourlyForecast>();
            if (!CheckIfSearchIsMadeByLoggedInUser(searchId)) return hourlyForecastsFromSearch;
            var hourlyForecast = from df in _dbContext.HourlyForecasts
                                 where df.SearchId.Equals(searchId)
                                 select df;
            hourlyForecastsFromSearch = hourlyForecast.ToList();
            return hourlyForecastsFromSearch;
        }
    }
}
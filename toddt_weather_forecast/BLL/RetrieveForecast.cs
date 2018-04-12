using System;
using System.Collections.Generic;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    /**
     * This class will have functions to help parse when the user makes either hourly or daily forecast requests.
     * This class also has functions to help parse the hourly or daily forecasts from prior searches.
     * TODO: Rename this, at first I was going to use the name "GetForecast" but since "Get" is the name of an HTTP
     * request I decided to changed to RetrieveForecast (for now).
     */ 
    public class RetrieveForecast
    {
        private const int ForecastsPerPage = 5;
        /**
         * by default the user will be returned hourly forecasts.
         * after the initial search is done the user has a choice to switch between hourly and daily forecasts.
         * if a user is logged in then save the search results then display.
         * if not logged in just display the results.
         */
        public HourlyForecastPagedList GetHourlyForecastsFromPostRequest(string address)
        {
            var geocodeApi = new GeocodeApi();
            geocodeApi.GetLatAndLngFromAddress(address);
            var latitudeFromAddress = geocodeApi.Latitude;
            var longitudeFromAddress = geocodeApi.Longitude;
            var searchId = geocodeApi.SearchId;
            var darkskyApi = new DarkskyApi();
            darkskyApi.ParseForecastResults(latitudeFromAddress, longitudeFromAddress, searchId);
            var parseHourlyForecast = new ParseHourlyForecast();
            const int currentPage = 1;
            var hourlyForecastPagedList = parseHourlyForecast.ToPagedList(darkskyApi.HourlyForecastList, currentPage, 
                ForecastsPerPage, geocodeApi);
            return hourlyForecastPagedList;
        }

        /**
         * this takes in a latitude and longitude pair as well as a page number in order to return a certain range of 
         * the hourly forecasts.
         */ 
        public HourlyForecastPagedList GetHourlyForecastsFromGetRequest(string latitude, string longitude, int page)
        {
            var darkskyApi = new DarkskyApi();
            darkskyApi.ParseForecastResults(latitude, longitude);
            var parseHourlyForecast = new ParseHourlyForecast();
            var hourlyForecastPagedList = parseHourlyForecast.ToPagedList(darkskyApi.HourlyForecastList, page, 
                ForecastsPerPage, null, latitude, longitude);
            return hourlyForecastPagedList;
        }

        /**
         * this function returns two serialized strings inside of the DailyForecastPagedList as it needs one for
         * the low temperatures and one for the high temperatures.
         */
        public DailyForecastPagedList GetDailyForecastsFromGetRequest(string latitude, string longitude, int page)
        {
            var darkskyApi = new DarkskyApi();
            darkskyApi.ParseForecastResults(latitude, longitude);
            var parseDailyForecast = new ParseDailyForecast();
            var dailyForecastPagedList = parseDailyForecast.ToPagedList(darkskyApi.DailyForecastList, page, 
                ForecastsPerPage, null, latitude, longitude);
            return dailyForecastPagedList;
        }

        /**
         * grabs a list of hourly forecasts from a specified search and then calls a function to get a paged
         * list of hourly forecasts within the page range with the searchId data field set.
         */ 
        public HourlyForecastPagedListFromSearch GetHourlyForecastsFromPreviousSearch(int page, int searchId)
        {
            var queryForecastsFromPreviousSearch = new QueryForecastsFromPreviousSearch();
            var hourlyForecastsFromSearch = queryForecastsFromPreviousSearch.GetHourlyForecastsFromSearchId(searchId);
            var parseHourlyForecastFromSearch = new ParseHourlyForecastFromSearch();
            var hourlyForecastPagedListFromSearch = parseHourlyForecastFromSearch.ToPagedList(hourlyForecastsFromSearch, 
                page, ForecastsPerPage, searchId);
            return hourlyForecastPagedListFromSearch;
        }

        /**
         * grabs a list of daily forecasts from a specified search and then calls a function to get a paged
         * list of daily forecasts within the page range with the searchId data field set.
         */
        public DailyForecastPagedListFromSearch GetDailyForecastsFromPreviousSearch(int page, int searchId)
        {
            var queryForecastsFromPreviousSearch = new QueryForecastsFromPreviousSearch();
            var dailyForecastsFromSearch = queryForecastsFromPreviousSearch.GetDailyForecastsFromSearchId(searchId);
            var parseDailyForecastFromSearch = new ParseDailyForecastFromSearch();
            var dailyForecastPagedListFromSearch = parseDailyForecastFromSearch.ToPagedList(dailyForecastsFromSearch,
                page, ForecastsPerPage, searchId);
            return dailyForecastPagedListFromSearch;
        }

    }
}
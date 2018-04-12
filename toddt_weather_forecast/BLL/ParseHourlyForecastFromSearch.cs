using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{    
    /**
     * A class providing a way to parse hourly forecasts from a prior search as well as assigning the associated
     * search id to the paged list.
     */ 
    public class ParseHourlyForecastFromSearch : ParseHourlyForecast
    {
        public HourlyForecastPagedListFromSearch ToPagedList(List<HourlyForecast> hourlyForecasts, int currentPage, 
            int forecastsPerPage, int searchId)
        {
            if (currentPage <= 0) return null;
            var hourlyForecastPagedListFromSearch = new HourlyForecastPagedListFromSearch();
            var totalNumberOfDailyForecasts = hourlyForecasts.Count();
            const string dailyForecastTitle = "Daily Forecasts";
            hourlyForecastPagedListFromSearch.TotalItemCount = totalNumberOfDailyForecasts;
            hourlyForecastPagedListFromSearch.hourlyForecastSearchId = searchId;
            ComputeNumberOfPages(hourlyForecastPagedListFromSearch, totalNumberOfDailyForecasts, forecastsPerPage);
            if (currentPage > hourlyForecastPagedListFromSearch.PageCount)
            {
                // this is if a user passes in a page number that is out of bounds.
                // this can also occur if the list is empty.
                return null;
            }
            GenerateSerializedHourlyForecastList(hourlyForecastPagedListFromSearch, hourlyForecasts, currentPage, 
                totalNumberOfDailyForecasts, forecastsPerPage);
            ComputeFirstItemOnPage(hourlyForecastPagedListFromSearch, forecastsPerPage, currentPage);
            ComputeLastItemOnPage(hourlyForecastPagedListFromSearch, forecastsPerPage, currentPage);
            FillPagedListDataMembers(hourlyForecastPagedListFromSearch, currentPage, forecastsPerPage, 
                dailyForecastTitle);

            return hourlyForecastPagedListFromSearch;
        }
    }
}
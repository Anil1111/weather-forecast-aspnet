using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    /**
     * A class to assist with parsing information from the daily forecast of a previous search into
     * a paged list with information filled as well as a search id being assigned.
     */ 
    public class ParseDailyForecastFromSearch : ParseDailyForecast
    {
        public DailyForecastPagedListFromSearch ToPagedList(List<DailyForecast> dailyForecasts, int currentPage, int forecastsPerPage, int searchId)
        {
            if (currentPage <= 0) return null;
            var dailyForecastPagedList = new DailyForecastPagedListFromSearch();
            var totalNumberOfDailyForecasts = dailyForecasts.Count();
            const string dailyForecastTitle = "Daily Forecasts";
            dailyForecastPagedList.TotalItemCount = totalNumberOfDailyForecasts;
            dailyForecastPagedList.dailyForecastSearchId = searchId;
            ComputeNumberOfPages(dailyForecastPagedList, totalNumberOfDailyForecasts, forecastsPerPage);
            if (currentPage > dailyForecastPagedList.PageCount)
            {
                // this is if a user passes in a page number that is out of bounds.
                // this can also occur if the list is empty.
                return null;
            }
            GenerateSerializedDailyForecastList(dailyForecastPagedList, dailyForecasts, currentPage, totalNumberOfDailyForecasts, forecastsPerPage);
            ComputeFirstItemOnPage(dailyForecastPagedList, forecastsPerPage, currentPage);
            ComputeLastItemOnPage(dailyForecastPagedList, forecastsPerPage, currentPage);
            FillPagedListDataMembers(dailyForecastPagedList, currentPage, forecastsPerPage, dailyForecastTitle);

            return dailyForecastPagedList;
        }
    }
}
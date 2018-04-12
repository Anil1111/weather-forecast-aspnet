using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    /**
     * Parse the hourly forecast objects into a IPagedList which can be used by a Razor view
     * to display hourly forecasts with pagination.
     */ 
    public class ParseHourlyForecast : ParseForecast
    {
        public HourlyForecastPagedList ToPagedList(List<HourlyForecast> hourlyForecasts, int currentPage, int pageSize, 
            GeocodeApi geocodeApi = null, String latitude = null, String longitude = null)
        {
            if (currentPage <= 0) return null;
            var hourlyForecastPagedList = new HourlyForecastPagedList();
            var totalNumberOfHourlyForecasts = hourlyForecasts.Count();
            const string hourlyForecastTitle = "Hourly Forecasts";
            hourlyForecastPagedList.TotalItemCount = totalNumberOfHourlyForecasts;
            ComputeNumberOfPages(hourlyForecastPagedList, totalNumberOfHourlyForecasts, pageSize);
            if (currentPage > hourlyForecastPagedList.PageCount)
            {
                // this is if a user passes in a page number that is out of bounds.
                // this can also occur if the list is empty.
                return null;
            }
            GenerateSerializedHourlyForecastList(hourlyForecastPagedList, hourlyForecasts, currentPage, 
                totalNumberOfHourlyForecasts, pageSize);

            ComputeFirstItemOnPage(hourlyForecastPagedList, pageSize, currentPage);
            ComputeLastItemOnPage(hourlyForecastPagedList, pageSize, currentPage);
            FillPagedListDataMembers(hourlyForecastPagedList, currentPage, pageSize, hourlyForecastTitle);
            FillLatitudeAndLongitudeValuesFromSearch(hourlyForecastPagedList, geocodeApi, latitude, longitude);

            return hourlyForecastPagedList;
        }

        /**
         * takes in all the hourly forecasts from the search and generates a sub set of these forecasts which are
         * serialized into the proper format for displaying on a graph. 
         */ 
        protected void GenerateSerializedHourlyForecastList(HourlyForecastPagedList hourlyForecastPagedList, 
            List<HourlyForecast> hourlyForecasts, int currentPage, int totalNumberOfHourlyForecasts, int pageSize)
        {
            GetHourlyForecastsFromRange(hourlyForecastPagedList, hourlyForecasts, currentPage, 
                totalNumberOfHourlyForecasts, pageSize);
            // serialize the list of hourly forecasts.
            hourlyForecastPagedList.SerializedHourlyForecast = JsonConvert.SerializeObject(hourlyForecastPagedList
                .CompressedHourlyForecasts);
        }

        /**
         * takes a list of all the hourly forecasts and returns a list with a sub set of these hourly forecast(s).
         */
        protected void GetHourlyForecastsFromRange(HourlyForecastPagedList hourlyForecastPagedList, IReadOnlyList
            <HourlyForecast> hourlyForecasts, int currentPage, int totalNumberOfForecasts, int itemsOnPage)
        {
            /**
             * the total number of forecasts minus 1 can be the upper bound (last page).
             * for example on the 10th page when paginating with a total count of 50 items, the final page only has
             * four items so we need an upper bound limit to stop or else we will go out of bounds on our list while
             * indexing.
             */
            var maximumIndex = totalNumberOfForecasts - 1;
            var startIndex = (currentPage - 1) * itemsOnPage;
            var endIndex = (currentPage * itemsOnPage) - 1;
            endIndex = Math.Min(endIndex, maximumIndex);
            while(startIndex <= endIndex)
            {
                var hourlyForecast = hourlyForecasts[startIndex++];
                var hourlyForecastViewModels = new ForecastViewModels
                {
                    FormattedTime = hourlyForecast.FormattedTime,
                    Temperature = hourlyForecast.Temperature
                };
                hourlyForecastPagedList.CompressedHourlyForecasts.Add(hourlyForecastViewModels);
            }
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    /**
     * A class providing specific functions for parsing daily forecast objects.
     */ 
    public class ParseDailyForecast : ParseForecast
    {
        /**
         * Returns an object which holds information for displaying the daily forecasts on a graph as well as
         * including information relevant for displaying information necessary for pagination of daily forecats.
         */
        public DailyForecastPagedList ToPagedList(List<DailyForecast> dailyForecasts, int currentPage, int forecastsPerPage, GeocodeApi geocodeApi = null, String latitude = null, String longitude = null)
        {
            if (currentPage <= 0) return null;
            var dailyForecastPagedList = new DailyForecastPagedList();
            var totalNumberOfDailyForecasts = dailyForecasts.Count();
            const string dailyForecastTitle = "Daily Forecasts";
            dailyForecastPagedList.TotalItemCount = totalNumberOfDailyForecasts;
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

            FillLatitudeAndLongitudeValuesFromSearch(dailyForecastPagedList, geocodeApi, latitude, longitude);

            return dailyForecastPagedList;
        }

        /**
         * takes in all the daily forecasts from the search and generates a sub set of these forecasts which are 
         * serialized into the proper format for displaying on a graph. 
         */
        protected void GenerateSerializedDailyForecastList(DailyForecastPagedList dailyForecastPagedList, 
            IReadOnlyList<DailyForecast> dailyForecasts, int currentPage, int totalNumberOfHourlyForecasts, int pageSize)
        {
            GetDailyForecastsFromRange(dailyForecastPagedList, dailyForecasts, currentPage, 
                totalNumberOfHourlyForecasts, pageSize);
            dailyForecastPagedList.SerializedLowTemperatures = JsonConvert.SerializeObject(
                dailyForecastPagedList.LowTemperatures);
            dailyForecastPagedList.SerializedHighTemperatures = JsonConvert.SerializeObject(
                dailyForecastPagedList.HighTemperatures);
        }

        /**
         * takes a list of all the hourly forecasts and returns a list with a sub set of these hourly forecast(s).
         */
        protected void GetDailyForecastsFromRange(DailyForecastPagedList dailyForecastPagedList, IReadOnlyList<
             DailyForecast> dailyForecasts, int currentPage, int totalNumberOfForecasts, int itemsOnPage)
        {
            /**
              * the total number of forecasts minus 1 can be the upper bound (last page).
              * for example on the 10th page when paginating with a total count of 50 items, the final page only has 
              * four items so we need an upper bound limit to stop or else we will go out of bounds on our list 
              * while indexing.
             */
            var maximumIndex = totalNumberOfForecasts - 1;
            var startIndex = (currentPage - 1) * itemsOnPage;
            var endIndex = (currentPage * itemsOnPage) - 1;
            endIndex = Math.Min(endIndex, maximumIndex);
            while (startIndex <= endIndex)
            {
                var dailyForecast = dailyForecasts[startIndex++];
                var dailyLowTemperature = new ForecastViewModels
                {
                    Temperature = dailyForecast.LowTemperature,
                    FormattedTime = dailyForecast.LowTemperatureFormattedTime
                };
                dailyForecastPagedList.LowTemperatures.Add(dailyLowTemperature);

                var dailyHighTemperature = new ForecastViewModels
                {
                    Temperature = dailyForecast.HighTemperature,
                    FormattedTime = dailyForecast.HighTemperatureFormattedTime
                };
                dailyForecastPagedList.HighTemperatures.Add(dailyHighTemperature);
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace toddt_weather_forecast.Models
{
    /**
     * This class represents an object which inherits from a IPagedList object which can be used for pagination on
     * the view which displays the hourly forecast for a search.
     */ 
    public class HourlyForecastPagedList : ForecastPagedList
    {
        public string YAxisLabel { get; }
        public string HourlyGraphTitle { get; }
        public string SerializedHourlyForecast { get; set; }
        public List<ForecastViewModels> CompressedHourlyForecasts { get; set; }

        public HourlyForecastPagedList() : base()
        {
            YAxisLabel = "Temperature (In Farenheit)";
            HourlyGraphTitle = "Hourly Forecast";
            CompressedHourlyForecasts = new List<ForecastViewModels>();
        }
    }
}
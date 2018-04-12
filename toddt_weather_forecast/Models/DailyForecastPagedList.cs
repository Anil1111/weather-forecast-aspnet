using System;
using System.Collections.Generic;

namespace toddt_weather_forecast.Models
{
    /**
     * This class represents an object which inherits from a IPagedList object which can be used for pagination on the
     * view which displays the daily forecast for a search.
     */
    public class DailyForecastPagedList : ForecastPagedList
    {
        public string LowTemperatureYAxisLabel { get; }
        public string HighTemperatureYAxisLabel { get; }
        public string LowTemperatureGraphTitle { get; }
        public string HighTemperatureGraphTitle { get; }
        public string SerializedLowTemperatures { get; set;}
        public string SerializedHighTemperatures { get; set; }

        public List<ForecastViewModels> LowTemperatures { get; set; }
        public List<ForecastViewModels> HighTemperatures { get; set; }

        public DailyForecastPagedList() : base()
        {
            LowTemperatureYAxisLabel = "Low temperature (In Farenheit)";
            HighTemperatureYAxisLabel = "High temperature (In Farenheit)";
            LowTemperatureGraphTitle = "Low Temperatures";
            HighTemperatureGraphTitle = "High Temperatures";
            LowTemperatures = new List<ForecastViewModels>();
            HighTemperatures = new List<ForecastViewModels>();
        }
    }
}

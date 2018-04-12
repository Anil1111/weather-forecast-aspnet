using System;

namespace toddt_weather_forecast.Models
{
    /**
     * An object holding the information of a daily forecast.
     */ 
    public class DailyForecast : Forecast
    {
        public decimal LowTemperature { get; set; }
        public DateTime LowTemperatureTime { get; set; }
        public string LowTemperatureFormattedTime { get; set; }
        public decimal HighTemperature { get; set; }
        public DateTime HighTemperatureTime { get; set; }
        public string HighTemperatureFormattedTime { get; set; }
    }
}
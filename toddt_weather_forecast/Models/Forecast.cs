using System;

namespace toddt_weather_forecast.Models
{
    /**
     * A base class for fields common between both hourly and daily forecasts.
     */ 
    public class Forecast
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime Time { get; set; }
        public string FormattedTime { get; set; }
        public int SearchId { get; set; }
        public Search Searches { get; set; }
    }
}
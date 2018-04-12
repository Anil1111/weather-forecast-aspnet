namespace toddt_weather_forecast.Models
{
    /**
     * Representation of an hourly forecast object.
     */ 
    public class HourlyForecast : Forecast
    {
        public decimal Temperature { get; set; }
    }
}
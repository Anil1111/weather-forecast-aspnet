
namespace toddt_weather_forecast.Models
{
    /**
     * This class will have extra data fields required to provide information to display hourly forecasts
     * from a previous user search.
     */ 
    public class HourlyForecastPagedListFromSearch : HourlyForecastPagedList
    {
        // the search Id for these hourly forecasts inside of this paged list.
        public int hourlyForecastSearchId { get; set; }
    }
}
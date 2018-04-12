using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace toddt_weather_forecast.Models
{
    public class DailyForecastPagedListFromSearch : DailyForecastPagedList
    {
        // the search Id for these hourly forecasts inside of this paged list.
        public int dailyForecastSearchId { get; set; }
    }
}
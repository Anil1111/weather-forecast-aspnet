using PagedList;
using System;

namespace toddt_weather_forecast.Models
{
    /**
     * base class which holds data members that are common between daily and hourly forecast objects.
     */ 
    public class ForecastPagedList : AbstractPagedList
    {
        public string XAxisLabel { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string LatitudeFromSearch { get; set; }
        public string LongitudeFromSearch { get; set; }

        public ForecastPagedList() {
            XAxisLabel = "Date";
        }
    }
}
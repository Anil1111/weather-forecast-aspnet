using System;
using System.Collections.Generic;

namespace toddt_weather_forecast.Models
{
    /**
     * A class to represent the search object holding related fields. The most important one is the id of the user who
     * made the search.
     */ 
    public class Search
    {
        public int Id { get; set; }
        public string Address { get; set; }
        /**
         * in the rails version I use a bigint to represent this unique ID.
         * for the AspNet Users table that is used to simplify the login the ID is a string (varchar).
         */
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<HourlyForecast> HourlyForecasts { get; set; }
        public virtual ICollection<DailyForecast> DailyForecasts { get; set; }
    }
}
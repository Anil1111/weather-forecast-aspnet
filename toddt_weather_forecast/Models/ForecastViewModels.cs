using System;
using System.Runtime.Serialization;

namespace toddt_weather_forecast.Models
{
    /**
     * A model used to assist with serialization from a Forecast object which holds the time and temperature for a 
     * daily or hourly forecast.
     */ 
    [DataContract]
    public class ForecastViewModels
    {
        [DataMember(Name = "y")]
        public decimal Temperature { get; set; }
        [DataMember(Name = "label")]
        public string FormattedTime { get; set; }
    }
}


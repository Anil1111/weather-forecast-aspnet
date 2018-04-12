using System.Collections.Generic;

namespace toddt_weather_forecast.Models
{
    /**
     * This class holds a list of previous searches and other information necessary for pagination.
     */ 
    public class PreviousSearchPagedList : AbstractPagedList
    {
        public List<Search> PreviousSearches { get; set; }

        public PreviousSearchPagedList()
        {
            PreviousSearches = new List<Search>();
        }
    }
}
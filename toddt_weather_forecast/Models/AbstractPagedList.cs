using PagedList;

namespace toddt_weather_forecast.Models
{
    /**
     * This is class defines all methods needed to be implemented from the IPagedList interface which will be used
     * when using a PagedList object for pagination in its child classes.
     */ 
    public abstract class AbstractPagedList : IPagedList
    {
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public int FirstItemOnPage { get; set; }
        public int LastItemOnPage { get; set; }
        public int PageCount { get; set; }
        public string ErrorMessage { get; set; }
    }
}
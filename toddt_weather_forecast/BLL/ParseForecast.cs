using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    /**
     * This class is a base class which holds methods that are commonly used for filling a PagedList data structure.
     */ 
    public class ParseForecast : FillPagedList
    {
        /**
         * fills in a variety of data members which can be used to assist with pagination on the view which displays the
         * hourly forecast results.
         * TODO: perhaps split this into more functions because different data members are being set.
         */
        protected void FillPagedListDataMembers(ForecastPagedList forecastPagedList, int currentPage, int pageSize, 
            string title = null)
        {
            forecastPagedList.PageNumber = currentPage;
            forecastPagedList.PageSize = pageSize;
            forecastPagedList.Count = pageSize;
            forecastPagedList.IsFirstPage = currentPage == 1;
            forecastPagedList.IsLastPage = currentPage == forecastPagedList.PageCount; 
            forecastPagedList.HasNextPage = !forecastPagedList.IsLastPage; 
            forecastPagedList.HasPreviousPage = !forecastPagedList.IsFirstPage; 
            if (title != null)
            {
                forecastPagedList.Title = title;
            }
        }

        /**
         * fill in the latitude and longitude data member values from the geocodeAPI object (which comes from a user
         * entered address), or from the latitude and longitude pair (which is provided from the URL, a GET request).
         */
        protected void FillLatitudeAndLongitudeValuesFromSearch(ForecastPagedList forecastPagedList, GeocodeApi 
            geocodeApi = null, string latitude = null, string longitude = null)
        {
            if (geocodeApi != null)
            {
                forecastPagedList.LatitudeFromSearch = geocodeApi.Latitude;
                forecastPagedList.LongitudeFromSearch = geocodeApi.Longitude;
            }
            else if ((latitude != null) && (longitude != null))
            {
                forecastPagedList.LatitudeFromSearch = latitude;
                forecastPagedList.LongitudeFromSearch = longitude;
            }
        }
    }
}
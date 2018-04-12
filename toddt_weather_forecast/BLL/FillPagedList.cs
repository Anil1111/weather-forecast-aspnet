using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    public class FillPagedList
    {
        /**
         * This computes the total number of pages needed to display all of the hourly forecasts.
         */
        protected void ComputeNumberOfPages(AbstractPagedList forecastPagedList, int totalSize, int itemsPerPage)
        {
            var totalRequiredPages = totalSize / itemsPerPage;
            var remainingHourlyForecasts = totalSize % itemsPerPage;
            /**
             * from observation we can see that if students per page divides evenly into the required pages then that is
             * all the required pages but if we have something that doesn't divide evenly then we still need an extra
             * page to account for those left over hourly forecast(s).
             * for example: assuming 3 hourly forecasts per page. if we have 9 total hourly forecasts then, 9 / 3 =
             * 3 required pages.
             * example #2: assuming 3 hourly forecasts per page. if we have 8 total hourly forecasts, 8 /3 = 2
             * (but we would still have 2 hourly forecasts that will not be displayed unless we allocate one extra page.
             */
            if (remainingHourlyForecasts != 0)
            {
                totalRequiredPages++;
            }
            forecastPagedList.PageCount = totalRequiredPages;
        }

        /**
         * computes the first item on the page, not based off 0-th indexing (such as in arrays).
         */
        protected void ComputeFirstItemOnPage(AbstractPagedList forecastPagedList, int pageSize, int pageNumber)
        {
            forecastPagedList.FirstItemOnPage = ((pageNumber - 1) * pageSize) + 1;
        }

        /**
         * computes the last item on the page (also not based off 0th indexing).
         */
        protected void ComputeLastItemOnPage(AbstractPagedList forecastPagedList, int pageSize, int pageNumber)
        {
            forecastPagedList.LastItemOnPage = (pageNumber * pageSize);
        }

        /**
         * fills in a variety of data members which can be used to assist with pagination on the view which displays the
         * hourly forecast results.
         * TODO: perhaps split this into more functions because different data members are being set.
         */
        protected void FillPagedListDataMembers(AbstractPagedList forecastPagedList, int currentPage, int pageSize)
        {
            forecastPagedList.PageNumber = currentPage;
            forecastPagedList.PageSize = pageSize;
            forecastPagedList.Count = pageSize;
            forecastPagedList.IsFirstPage = currentPage == 1;
            forecastPagedList.IsLastPage = currentPage == forecastPagedList.PageCount; 
            forecastPagedList.HasNextPage = !(forecastPagedList.IsLastPage); 
            forecastPagedList.HasPreviousPage = !(forecastPagedList.IsFirstPage);
        }
    }
}
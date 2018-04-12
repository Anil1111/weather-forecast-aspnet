using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    /**
     * This class provides functions to grab searches made by previous users.
     */ 
    public class ParsePreviousSearch : FillPagedList
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly int _pageSize;
        private string NoPreviousSearchesMessage;

        public ParsePreviousSearch()
        {
            _dbContext = new ApplicationDbContext();
            _pageSize = 5;
            NoPreviousSearchesMessage = "You have not made any searches yet";
        }

        /**
         * This function queries the database to get prior searches from the logged in user and fills in the list
         * of up to 5 previous searches given the page.
         */ 
        public PreviousSearchPagedList GetPriorSearchesFromLoggedInUser(int currentPage)
        {
            // TODO: refactor to use the Utility isLoggedIn helper function.don
            var isLoggedIn = HttpContext.Current?.User?.Identity?.IsAuthenticated;
            var loggedInUserId = HttpContext.Current?.User?.Identity?.GetUserId<int>();
            if (loggedInUserId <= 0) return new PreviousSearchPagedList();
            var searchesByCurrentLoggedInUser = from userSearches in _dbContext.Searches
                where userSearches.UserId.Equals(loggedInUserId)
                select userSearches;
            var previousSearchPagedList = GetPreviousSearchPagedList(searchesByCurrentLoggedInUser, currentPage);
            if(previousSearchPagedList.Count == 0)
            {
                previousSearchPagedList.ErrorMessage = NoPreviousSearchesMessage;
            }
            return previousSearchPagedList;
        }

        /**
         * This function has two main purposes.
         * The first is to fill the list of previous searches up to five prior searches based off the current page.
         * The second is to call helper methods to fill in data fields that assist with pagination of the paged list.
         */ 
        private PreviousSearchPagedList GetPreviousSearchPagedList(IQueryable<Search> searchesByLoggedInUser, 
            int currentPage)
        {
            if (currentPage <= 0) return null;
            var previousSearchPagedList = new PreviousSearchPagedList();
            var totalNumberOfSearchesByLoggedInUser = searchesByLoggedInUser.Count();
            previousSearchPagedList.TotalItemCount = totalNumberOfSearchesByLoggedInUser;
            ComputeNumberOfPages(previousSearchPagedList, totalNumberOfSearchesByLoggedInUser, _pageSize);
            GetPreviousSearchesFromRange(previousSearchPagedList, searchesByLoggedInUser, currentPage);
            ComputeFirstItemOnPage(previousSearchPagedList, _pageSize, currentPage);
            ComputeLastItemOnPage(previousSearchPagedList, _pageSize, currentPage);
            FillPagedListDataMembers(previousSearchPagedList, currentPage, _pageSize);
            return previousSearchPagedList;
        }

        /**
         * This function fills in the PreviousSearches data field which holds up to five previous searches
         * dependent on the current page passed in as well as the total number of prior searches.
         */
        private void GetPreviousSearchesFromRange(PreviousSearchPagedList previousSearchPagedList,
            IQueryable<Search> searchesByLoggedInUser, int currentPage)
        {
            var startIndex = (currentPage - 1) * _pageSize;
            var searchesInRangeOfPage = searchesByLoggedInUser.OrderByDescending(search => search.CreatedAt).Skip(startIndex).Take(_pageSize);
            previousSearchPagedList.PreviousSearches = searchesInRangeOfPage.ToList();
        }
    }
}
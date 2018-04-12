using System;
using System.Net;
using System.Web;
using System.IO;
using toddt_weather_forecast.Models;
using Newtonsoft.Json.Linq;

namespace toddt_weather_forecast.BLL
{
    /**
     * A class that allows the user to make calls to the Google Geocode API which allows for the translation of an 
     * entered address to generate a pair of latitude and longitude coordinates stored as a string.
     */ 
    public class GeocodeApi
    {
        private const string GoogleMapsGeocodeApiKey = "GOOGLE_MAPS_GC_KEY";
        private readonly ApplicationDbContext _dbContext;
        private Utility Utility;

        public int SearchId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public GeocodeApi()
        {
            _dbContext = new ApplicationDbContext();
            SearchId = 0;
            Latitude = "";
            Longitude = "";
            Utility = new Utility();
        }

        /**
         * creates an Http GET request to get a json object that holds information such as
         * the latitude and longitude pair for a specified address.
         */ 
        public string CreateGeocodeRequestURL(string address)
        {
            string googleGeocodeApiKey = Environment.GetEnvironmentVariable(GoogleMapsGeocodeApiKey);
            string uriEncodedAddress = HttpUtility.UrlEncode(address);
            string requestUrl = "https://maps.googleapis.com/maps/api/geocode/json?address=" + uriEncodedAddress + 
                "&key=" + googleGeocodeApiKey;
            WebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = null;
            // Create a request. 
            var request = WebRequest.Create(requestUrl);
            try
            {
                // Get the response.  
                response = request.GetResponse();
                // Get the stream containing content returned by the server.  
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                reader = new StreamReader(dataStream);
                // Read the content.  
                responseFromServer = reader.ReadToEnd();
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
            }
            finally
            {
                // Clean up the streams and the response.  
                reader?.Close();
                response?.Close();
                dataStream?.Close();
            }
            return responseFromServer;
        }

        /**
         * Makes an HTTP request to get a json object holding the latitude and longitude coordinates.
         * This method will also save the search if the user is logged in as well as assign the search id
         * which can be used to save forecast results with the associated search id.
         */ 
        public void GetLatAndLngFromAddress(string address)
        {
            var geocodeSearchResult = CreateGeocodeRequestURL(address);
            if (geocodeSearchResult == null) return;
            var geocodeSearchResultAsJson = JObject.Parse(geocodeSearchResult);
            StoreUserSearchIfLoggedIn(address);
            SetLocationDataMembers(geocodeSearchResultAsJson);
        }

        /**
         * method that stores information about the search if the user is logged in
         */ 
        public void StoreUserSearchIfLoggedIn(String address)
        {
            if (!Utility.CheckIfUserLoggedIn()) return;
            DateTime dateTime = DateTime.Now;
            var search = new Search
            {
                Address = address,
                UserId = Utility.GetIdOfCurrentLoggedInUser(),
                CreatedAt = dateTime,
                UpdatedAt = dateTime
            };
            var searchByLoggedInUser = _dbContext.Searches.Add(search);
            _dbContext.SaveChanges();
            if(searchByLoggedInUser.Id > 0)
            {
                SearchId = searchByLoggedInUser.Id;
            }
        }

        /**
         * goes through the results array of the passed in JSON contents and grabs the latitude and longitude values.
         */ 
        public void SetLocationDataMembers(JObject geocodeSearchResults)
        {
            var resultsArray = (JArray)geocodeSearchResults["results"];
            if (!(resultsArray?.Count > 0)) return;
            var geometry = resultsArray[0]["geometry"];
            var location = geometry?["location"];
            if (location == null) return;
            if (location["lat"] != null) Latitude = location["lat"].ToString();
            if (location["lng"] != null) Longitude = location["lng"].ToString();
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using toddt_weather_forecast.Models;

namespace toddt_weather_forecast.BLL
{
    /**
     * This is a class which helps make DarkSky API calls which get hourly and daily forecasts
     * by grabbing the API key from an environment variable.
     */ 
    public class DarkskyApi
    {
        private const string DarkSkyApiKey = "DARK_SKY_KEY";
        private readonly ApplicationDbContext _dbContext;
        private Utility Utility;
        public string HourlySummary { get; set; }
        public string DailySummary { get; set; }
        public List<HourlyForecast> HourlyForecastList { get; set; }
        public List<DailyForecast> DailyForecastList { get; set; }

        public DarkskyApi()
        {
            _dbContext = new ApplicationDbContext();
            HourlyForecastList = new List<HourlyForecast>();
            DailyForecastList = new List<DailyForecast>();
            Utility = new Utility();
        }

        public string GetForecast(string latitude, string longitude)
        {
            if (latitude == null) throw new ArgumentNullException(nameof(latitude));
            if (latitude == null) throw new ArgumentNullException(nameof(latitude));
            string darkSkyApiKey = Environment.GetEnvironmentVariable(DarkSkyApiKey);
            var requestUrl = "https://api.darksky.net/forecast/" + darkSkyApiKey + "/" + latitude + "," + longitude 
                + "?exclude=minutely,flags";
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
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
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
         * fill the fields of this class holding forecast information and daily/hourly summaries.
         */
        public void ParseForecastResults(string latitude, string longitude, int searchId = 0)
        {
            var results = GetForecast(latitude, longitude);
            var resultsAsJson = JObject.Parse(results);
            SetSummaries(resultsAsJson);
            FillHourlyForecastList(resultsAsJson, searchId);
            FillDailyForecastList(resultsAsJson, searchId);
        }

        /**
         * set the fields holding the hourly and daily summaries.
         */
        private void SetSummaries(JObject contents)
        {
            if (contents == null) return;
            var hourlyContent = contents["hourly"];
            if (hourlyContent != null)
            {
                var hourlySummary = hourlyContent["summary"];
                HourlySummary = hourlySummary?.ToString() ?? "";
            }
            var dailyContent = contents["daily"];
            if (dailyContent == null) return;
            var dailySummary = dailyContent["summary"];
            DailySummary = dailySummary?.ToString() ?? "";
        }

        /**
         * goes through the json contents from the DarkSky call and fills the list with HourlyForecast items.
         */
        private void FillHourlyForecastList(JObject contents, int searchId = 0)
        {
            if (contents == null) return;
            var hourlyContents = contents["hourly"];
            var hourlyData = hourlyContents?["data"];
            if (hourlyData == null) return;
            var isUserLoggedIn = Utility.CheckIfUserLoggedIn();
            var hourlyDataArray = (JArray)hourlyData;
            foreach (var hourlyDataContents in hourlyDataArray)
            {
                // TODO: move the content of the body into its own private function below (is this necessary?)
                var hourlyForecast = new HourlyForecast();
                if (hourlyDataContents["time"] != null)
                {
                    FillTimeFields(hourlyForecast, hourlyDataContents["time"]);
                }
                if (hourlyDataContents["summary"] != null)
                {
                    hourlyForecast.Summary = hourlyDataContents["summary"].ToString();
                }
                if (hourlyDataContents["temperature"] != null)
                {
                    hourlyForecast.Temperature = hourlyDataContents["temperature"].ToObject<decimal>();
                }
                if (searchId > 0 && isUserLoggedIn)
                {
                    hourlyForecast.SearchId = searchId;
                    _dbContext.HourlyForecasts.Add(hourlyForecast);
                    _dbContext.SaveChanges();
                }
                HourlyForecastList.Add(hourlyForecast);
            }
        }

        /**
         * goes through the json contents and fills in the list holding the DailyForecast items.
         */
        private void FillDailyForecastList(JObject contents, int searchId = 0)
        {
            if (contents == null) return;
            var dailyContents = contents["daily"];
            var dailyData = dailyContents?["data"];
            if (dailyData == null) return;
            var isUserLoggedIn = Utility.CheckIfUserLoggedIn();
            var dailyDataArray = (JArray)dailyData;
            foreach (var dailyDataContents in dailyDataArray)
            {
                // TODO: move the content of the body into its own private function below (is this necessary?)
                var dailyForecast = new DailyForecast();
                if (dailyDataContents["time"] != null)
                {
                    FillTimeFields(dailyForecast, dailyDataContents["time"]);
                }
                if (dailyDataContents["summary"] != null)
                {
                    dailyForecast.Summary = dailyDataContents["summary"].ToString();
                }
                if (dailyDataContents["temperatureLow"] != null)
                {
                    dailyForecast.LowTemperature = dailyDataContents["temperatureLow"].ToObject<decimal>();
                }
                if (dailyDataContents["temperatureLowTime"] != null)
                {
                    var lowTemperaturetime = GetModifiedDateTime(dailyDataContents["temperatureLowTime"].
                        ToObject<int>());
                    dailyForecast.LowTemperatureTime = lowTemperaturetime;
                    dailyForecast.LowTemperatureFormattedTime = ConvertDateTimeToFormattedString(
                        lowTemperaturetime);
                }
                if (dailyDataContents["temperatureHigh"] != null)
                {
                    dailyForecast.HighTemperature = dailyDataContents["temperatureHigh"].ToObject<decimal>();
                }
                if (dailyDataContents["temperatureHighTime"] != null)
                {
                    var highTemperaturetime = GetModifiedDateTime(dailyDataContents["temperatureHighTime"]
                        .ToObject<int>());
                    dailyForecast.HighTemperatureTime = highTemperaturetime;
                    dailyForecast.HighTemperatureFormattedTime = ConvertDateTimeToFormattedString(
                        highTemperaturetime);
                }
                if(searchId > 0 && isUserLoggedIn)
                {
                    dailyForecast.SearchId = searchId;
                    _dbContext.DailyForecasts.Add(dailyForecast);
                    _dbContext.SaveChanges();
                }
                DailyForecastList.Add(dailyForecast);
            }
        }

        /**
         * helper method to return the datetime object in a specified string format.
         */
        private string ConvertDateTimeToFormattedString(DateTime dateTime)
        {
            return dateTime.ToString("MMMM dd, yyyy") + " at " + dateTime.ToString("h:mm tt");
        }

        /**
         * helper method to fill the time and formatted time fields by adding seconds to a time starting from
         * Jan 1st, 1970 at 12AM.
         */ 
        private void FillTimeFields(Forecast forecast, JToken contents)
        {
            DateTime currentTime = DateTime.Now;
            forecast.CreatedAt = currentTime;
            forecast.UpdatedAt = currentTime;
            var timeInSeconds = contents.ToObject<int>();
            var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            time = time.AddSeconds(timeInSeconds);
            forecast.Time = time;
            forecast.FormattedTime = ConvertDateTimeToFormattedString(time);
        }

        /**
         * helper method to get a DateTime object with time (in seconds) added to it from Jan 1st, 1970 at 12AM.
         */ 
        private DateTime GetModifiedDateTime(int timeInSeconds)
        {
            var modifiedDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modifiedDateTime = modifiedDateTime.AddSeconds(timeInSeconds);
            return modifiedDateTime;
        }
    }
}
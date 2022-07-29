using System.Reflection.Metadata;
using System.Reflection;
using System.Reflection.Emit;
using WeatherAPI.Interface;
using WeatherAPI.Models;
using WeatherApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherAPI.Services
{
    public class WeatherService : IWeatherService
    {
        static string? _baseUrl;
        static string? _apiKey;
        
        public WeatherService(string baseUrl, string apiKey)
        {
            _baseUrl = baseUrl;
            _apiKey = apiKey;

        }


        public async Task<Forecast> GetCurrentWeather(Location location)
        {
            string url = $"{_baseUrl}?lat={location.Lat}&lon={location.Lon}&exclude=minutely,hourly,daily&units=imperial&appid={_apiKey}";
            return await GetResponse(url);
        }

        public async Task<Forecast?> GetResponse(string address)
        {
            string result;
            try
            {
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(address);
                response.EnsureSuccessStatusCode();
            
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    if (result != null)
                    {
                        Forecast forecast = Forecast.Create(result);
                        return forecast;


                    }
                }
                var errorString = await response.Content.ReadAsStringAsync();
                return null;
            }
            catch(Exception ex)
            {
       
                return null;
            }
            
        }
    }
}
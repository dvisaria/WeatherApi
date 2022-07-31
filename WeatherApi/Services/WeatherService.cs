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
        static HttpClient? _client;
        
        public WeatherService(IConfiguration config, HttpClient client)
        {
            _baseUrl = config.GetSection("WeatherApi").GetValue<string>("BaseUrl");
            _apiKey = config.GetSection("WeatherApi").GetValue<string>("ApiKey");

            _client = client;

        }


        public async Task<Forecast?> GetCurrentWeather(Location location)
        {
            string url = $"{_baseUrl}?lat={location.Lat}&lon={location.Lon}&exclude=minutely,hourly,daily&units=imperial&appid={_apiKey}";
            return await GetResponse(url);
        }

        public async Task<Forecast?> GetResponse(string address)
        {
            string result;
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                HttpResponseMessage response = await _client.GetAsync(address);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                response.EnsureSuccessStatusCode();
            
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    if (result != null)
                    {
                        Forecast? forecast = Forecast.Create(result);
                        return forecast;


                    }
                }
                var errorString = await response.Content.ReadAsStringAsync();
                return null;
            }
            catch
            {
                return null;
            }
            
        }
    }
}
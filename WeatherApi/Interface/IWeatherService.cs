using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using WeatherApi.Models;
using WeatherAPI.Models;

namespace WeatherAPI.Interface
{
    public interface IWeatherService
    {
         public Task<Forecast> GetCurrentWeather(Location location);
    }
}
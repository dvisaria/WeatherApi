using WeatherAPI.Models;
using WeatherAPI.Interface;
using System.Web.Http.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WeatherApi.Models;
using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _service;


    
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService service)
    {
        _logger = logger;
        _service = service;
    }




    [HttpGet]
    public async Task<IActionResult> GetCurrentWeather([FromQuery] Location location)
    {
        if (ModelState.IsValid) {    
            var result = await _service.GetCurrentWeather(location);
            if( result != null)
                return Ok(result);

            _logger.LogError("Result not found", ModelState);
            return NotFound();
        }

         _logger.LogError("Request Validation Error", ModelState);
         return BadRequest(ModelState);
      
    }
}

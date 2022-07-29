using WeatherAPI.Controllers;
using Moq;
using WeatherAPI.Interface;
using Microsoft.Extensions.Logging;
using WeatherApi.Models;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace WeatherApiTest1
{
    public class Tests
    {
        Mock<IWeatherService> mockService;
        Mock<ILogger<WeatherForecastController>> mockLogger;


        WeatherForecastController controller;

        private void SimulateValidation(Location model)
        {
            // mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationResultList = new List<ValidationResult>();
            bool b1 = Validator.TryValidateObject(model, new ValidationContext(model), validationResultList, true);
            foreach (var validationResult in validationResultList)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        [SetUp]
        public void Setup()
        {
            mockService = new Mock<IWeatherService>();
            mockLogger = new Mock<ILogger<WeatherForecastController>>();


            controller = new WeatherForecastController(mockLogger.Object, mockService.Object);
        }

        [Test]
        public void InvalidLatLon()
        {
            
            // Act
            Location loc = new Location();
            SimulateValidation(loc);
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);

            // Arrange
            controller = new WeatherForecastController(mockLogger.Object, mockService.Object);

            // Act
            loc.Lat = 0.0;
            SimulateValidation(loc);
            actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);

            // Arrange
            controller = new WeatherForecastController(mockLogger.Object, mockService.Object);

            // Act
            loc.Lon = 0.0;
            SimulateValidation(loc);
            actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(actionResult.Result);

            // Arrange
            controller = new WeatherForecastController(mockLogger.Object, mockService.Object);

            // Act
            loc.Lat = -100;
            loc.Lon = 181;
            SimulateValidation(loc);
            actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);

            // Act
            loc.Lat = 100;
            loc.Lon = -181;
            SimulateValidation(loc);
            actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);
        }


        [Test]
        public void GetCurrentWeather()
        {
            // Arrange
            var mockService = new Mock<IWeatherService>();
            var mockLogger = new Mock<ILogger<WeatherForecastController>>();
            Forecast forecast = new Forecast();
            forecast.Condition = "Clouds";
            forecast.Description = "scattered clouds";
            double temp = 64.45;
            forecast.Temperature = temp.ToString() + " °F";
            forecast.SetFeels(temp);


            Alert alert = new Alert();
            alert.SenderName = "NWS Fairbanks (Northern Alaska - Fairbanks)";
            alert.Event = "Special Weather Statement";
            alert.Description = "...Frost Possible Tonight In Low Lying Areas Of The Eastern\nInterior...\nCold air now in place across the Eastern Interior, combined with\nclearing skies tonight, will cause very cool temperatures across\nthe Eastern Interior tonight. Valley locations that are prone to\nsummer frost will likely see frost tonight, such as areas near\nGoldstream Creek, the Chatanika River, the Upper Chena River, and\nthe Fortymile River as well as Denali Park.\nPeople in these low lying areas with frost sensitive plants\nshould take necessary precautions to protect them tonight.";
            //alert.Start = new DateTime("2022-07-28T15:24:00-04:00");
            //alert.End = new DateTime("2022-07-29T10:00:00-04:00");
            forecast.alerts.Add(alert);

            Location loc = new Location(64.835365, -147.776749);
            mockService.Setup(x => 
            x.GetCurrentWeather(loc))
                .Returns(Task.FromResult(forecast)
            );


            var controller = new WeatherForecastController(mockLogger.Object, mockService.Object);
            
            SimulateValidation(loc);
            // Act
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);
            var contentResult = (OkObjectResult)actionResult.Result;            

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOf<OkObjectResult>(contentResult);
            Assert.IsInstanceOf<Forecast>(contentResult.Value);
        }
    }
}

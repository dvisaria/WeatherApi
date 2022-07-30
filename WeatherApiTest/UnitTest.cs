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

            Forecast forecast = new Forecast();
            Location loc = new Location(64.835365, -147.776749);
            mockService.Setup(x =>
            x.GetCurrentWeather(loc))
                .Returns(Task.FromResult(forecast)
            );
            controller = new WeatherForecastController(mockLogger.Object, mockService.Object);
        }

        [Test]
        public void NullLatLonValidation()
        {
            
            // Act
            Location loc = new Location();
            SimulateValidation(loc);
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);            
        }

        [Test]
        public void NullLatValidation()
        {

            // Act
            Location loc = new Location();
            loc.Lat = 0;
            SimulateValidation(loc);
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);
        }

        [Test]
        public void NullLonValidation()
        {

            // Act
            Location loc = new Location();
            loc.Lat = 0;
            SimulateValidation(loc);
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);
        }

        [Test]
        public void LatRangeValidation()
        {

            // Act
            Location loc = new Location();
            loc.Lat = -90.00001;
            loc.Lon = 0;
            SimulateValidation(loc);
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);
        }

        [Test]
        public void LonRangeValidation()
        {

            // Act
            Location loc = new Location();
            loc.Lat = 0;
            loc.Lon = 180.01;
            SimulateValidation(loc);
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);
        }


        [Test]
        public void NotFoundResponse()
        {
            Forecast forecast = null;

            Location loc = new Location();
            loc.Lat = 0;
            loc.Lon = 0;
            mockService.Setup(x =>
            x.GetCurrentWeather(loc))
                .Returns(Task.FromResult(forecast)
            );

            // Act            
            SimulateValidation(loc);
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(actionResult.Result);


        }


        [Test]
        public void GetCurrentWeather()
        {
            // Arrange
            Location loc = new Location(64.835365, -147.776749);
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
            forecast.alerts.Add(alert);

            mockService.Setup(x =>
            x.GetCurrentWeather(loc))
                .Returns(Task.FromResult(forecast)
            );

            // Act
            SimulateValidation(loc);
            Task<IActionResult> actionResult = controller.GetCurrentWeather(loc);
            var contentResult = (OkObjectResult)actionResult.Result;            

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOf<OkObjectResult>(contentResult);
            Assert.IsInstanceOf<Forecast>(contentResult.Value);
            Forecast result = contentResult.Value as Forecast;
            Assert.AreEqual(result.Temperature, forecast.Temperature);
            Assert.AreEqual(result.Feels, forecast.Feels);
            Assert.AreEqual(result.Condition, forecast.Condition);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForecastApp.ForecastAppModels;
using ForecastApp.Logger;
using ForecastApp.OpenWeatherMapModels;
using ForecastApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ForecastApp.Controllers
{
    public class ForecastAppController : Controller
    {
        private readonly IForecastRepository _forecastRepository;
		private readonly ILogger _logger;

		// Dependency Injection
		public ForecastAppController(IForecastRepository forecastAppRepo, ILogger logger)
		{
			_forecastRepository = forecastAppRepo;
			_logger = logger;
		}

		// GET: ForecastApp/SearchCity
		public IActionResult SearchCity()
        {
            var viewModel = new SearchCity();
			_logger.Info($"ForecastAppController GET: ForecastApp/SearchCity. City {viewModel.CityName} ​​found.");
            return View(viewModel);
        }

        // POST: ForecastApp/SearchCity
        [HttpPost]
        public IActionResult SearchCity(SearchCity model)
        {
			// If the model is valid, consume the Weather API to bring the data of the city

			_logger.Info("ForecastAppController POST: ForecastApp/SearchCity started.");

			if (ModelState.IsValid) {
                return RedirectToAction("City", "ForecastApp", new { city = model.CityName });
            }

			_logger.Info("ForecastAppController POST: ForecastApp/SearchCity started.");
			return View(model);
        }

        // GET: ForecastApp/City
        public IActionResult City(string city)
        {
            // Consume the OpenWeatherAPI in order to bring Forecast data in our page.
            WeatherResponse weatherResponse = _forecastRepository.GetForecast(city);
            City viewModel = new City();

            if (weatherResponse != null)
            {
                viewModel.Name = weatherResponse.Name;
                viewModel.Humidity = weatherResponse.Main.Humidity;
                viewModel.Pressure = weatherResponse.Main.Pressure;
                viewModel.Temp = weatherResponse.Main.Temp;
                viewModel.Weather = weatherResponse.Weather[0].Main;
                viewModel.Wind = weatherResponse.Wind.Speed;
            }

			var message = new StringBuilder();
			message.AppendLine($"ForecastAppController GET: ForecastApp/City. Forecast data ​​found for {viewModel.Name}.");
			message.Append($"Humidity: {viewModel.Humidity}, Pressure: {viewModel.Pressure}, Weather: {viewModel.Weather}, Wind:{viewModel.Wind}");

			_logger.Info(message.ToString());
			
			return View(viewModel);
        }
    }
}
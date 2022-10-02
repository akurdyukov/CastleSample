using System;
using System.Collections.Generic;
using System.Linq;
using CastleSample.Models;
using CastleSample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CastleSample.Controllers
{
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
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Getting forecast");
            return _service.GetForecast().ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CastleSample.Models;
using Microsoft.Extensions.Logging;

namespace CastleSample.Services;

public class RandomWeatherService: IWeatherService
{
    private readonly ILogger<RandomWeatherService> _logger;
    
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public RandomWeatherService(ILogger<RandomWeatherService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<WeatherForecast> GetForecast()
    {
        _logger.LogDebug("Generating random weather forecast");
        
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        });
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CastleSample.Models;
using CastleSample.Storage;
using Microsoft.Extensions.Logging;

namespace CastleSample.Services;

public class RandomWeatherService: IWeatherService
{
    private readonly ILogger<RandomWeatherService> _logger;
    private readonly IWeatherForecastRepository _repository;
    
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public RandomWeatherService(ILogger<RandomWeatherService> logger, IWeatherForecastRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IEnumerable<WeatherForecast> GetForecast()
    {
        _logger.LogDebug("Generating random weather forecast");
        
        var rng = new Random();
        var items = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }).ToImmutableList();
        _repository.SaveAll(items);
        return items;
    }
}
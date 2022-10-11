using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CastleSample.Models;
using CastleSample.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CastleSample.Services;

public class RandomWeatherService: IWeatherService
{
    private readonly ILogger<RandomWeatherService> _logger;
    private readonly IWeatherForecastRepository _repository;
    private readonly RandomWeatherOptions _options;

    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public RandomWeatherService(ILogger<RandomWeatherService> logger, IWeatherForecastRepository repository, 
        IOptions<RandomWeatherOptions> options)
    {
        _logger = logger;
        _repository = repository;
        _options = options.Value;
    }

    public IEnumerable<WeatherForecast> GetForecast()
    {
        _logger.LogDebug("Generating random weather forecast");
        
        var rng = new Random();
        var items = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(_options.MinTemperature, _options.MaxTemperature),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }).ToImmutableList();
        _repository.SaveAll(items);
        return items;
    }
}

public class RandomWeatherOptions
{
    public int MinTemperature { get; set; }
    public int MaxTemperature { get; set; }
}

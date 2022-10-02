using System.Collections.Generic;
using CastleSample.Models;

namespace CastleSample.Services;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetForecast();
}
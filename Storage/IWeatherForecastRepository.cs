using System.Collections.Generic;
using CastleSample.Models;

namespace CastleSample.Storage;

public interface IWeatherForecastRepository
{
    IReadOnlyList<WeatherForecast> GetAll();
    void SaveAll(IReadOnlyList<WeatherForecast> entities);
}

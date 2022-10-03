using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CastleSample.Models;
using NHibernate;

namespace CastleSample.Storage;

public class HibernateWeatherForecastRepository: IWeatherForecastRepository
{
    private readonly ISessionFactory _factory;

    public HibernateWeatherForecastRepository(ISessionFactory factory)
    {
        _factory = factory;
    }

    public IReadOnlyList<WeatherForecast> GetAll()
    {
        using var session = _factory.OpenSession();
        return session.Query<WeatherForecast>()
            .OrderBy(f => f.Date)
            .ToImmutableList();
    }

    public void SaveAll(IReadOnlyList<WeatherForecast> entities)
    {
        using var session = _factory.OpenSession();
        using var transaction = session.BeginTransaction();
        foreach (var entity in entities)
        {
            session.Save(entity);
        }
    }
}
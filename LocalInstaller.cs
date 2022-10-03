using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CastleSample.Services;
using CastleSample.Storage;
using NHibernate.Connection;

namespace CastleSample;

/// <summary>
/// Local Windsor installer class
/// </summary>
public class LocalInstaller: IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        container.Register(
            Component.For<InjectedConnectionProvider, IConnectionProvider>().ImplementedBy<InjectedConnectionProvider>(),
            
            Component.For<IWeatherService>().ImplementedBy<RandomWeatherService>(),
            Component.For<IWeatherForecastRepository>().ImplementedBy<HibernateWeatherForecastRepository>()
        );
    }
}

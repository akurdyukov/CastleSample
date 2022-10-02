using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CastleSample.Services;

namespace CastleSample;

/// <summary>
/// Local Windsor installer class
/// </summary>
public class LocalInstaller: IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        container.Register(
            Component.For<IWeatherService>().ImplementedBy<RandomWeatherService>()
        );
    }
}

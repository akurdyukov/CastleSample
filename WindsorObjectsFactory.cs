using System;
using Castle.Windsor;
using NHibernate.Bytecode;

namespace CastleSample;

public class WindsorObjectsFactory : IObjectsFactory
{
    private readonly IWindsorContainer _container;

    public WindsorObjectsFactory(IWindsorContainer container)
    {
        this._container = container;
    }

    public object CreateInstance(Type type)
    {
        return _container.Kernel.HasComponent(type) ? _container.Resolve(type) : Activator.CreateInstance(type);
    }

    public object CreateInstance(Type type, bool nonPublic)
    {
            
        return _container.Kernel.HasComponent(type) ? _container.Resolve(type) : Activator.CreateInstance(type, nonPublic);
    }

    public object CreateInstance(Type type, params object[] ctorArgs)
    {

        return Activator.CreateInstance(type, ctorArgs);
    }
}
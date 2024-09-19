using System.Linq.Expressions;
using System.Reflection;

namespace IoCContainer;

public class IoCContainer
{
    private readonly Dictionary<Type, Type> _registeredServices = [];

    public void RegisterService<TInterface, TService>()
        where TInterface : class
        where TService : class, TInterface
    {
        _registeredServices.Add(typeof(TInterface), typeof(TService));
    }

    public void RegisterService<TService>()
        where TService : class
    {
        _registeredServices.Add(typeof(TService), typeof(TService));
    }

    public Type GetRegisteredService<TService>()
    {
        return GetRegisteredService<TService>(typeof(TService));
    }

    public Type GetRegisteredService<TService>(Type service)
    {
        if (!_registeredServices.ContainsKey(service))
        {
            throw new NullReferenceException($"Cannot resolve type {typeof(TService)}.");
        }

        return _registeredServices[service];
    }

    public TService Resolve<TService>() where TService : class
    {
        return Resolve<TService>(typeof(TService));
    }

    public TService Resolve<TService>(Type service) where TService : class
    {
        Type type = GetRegisteredService<TService>(service);

        var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        ConstructorInfo? chosenCtor = null;

        foreach (var ctor in ctors)
        {
            chosenCtor = ctor;
            break;
        }

        if (null == chosenCtor)
        {
            throw new NullReferenceException($"Type '{type}' has no public constructor that can be resolved.");
        }

        var parameters = new List<object>();

        foreach (var paramInfo in chosenCtor.GetParameters())
        {
            parameters.Add(
                Resolve<object>(paramInfo.ParameterType)
            );
        }

        return (TService) chosenCtor.Invoke([.. parameters]);
    }
}

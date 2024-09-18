using System.Linq.Expressions;

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
        if (!_registeredServices.ContainsKey(typeof(TService)))
        {
            throw new NullReferenceException($"Cannot resolve type {typeof(TService)}.");
        }

        return _registeredServices[typeof(TService)];
    }

    public TService Resolve<TService>()
        where TService : class
    {
        return CreateServiceConstructor<TService>(
            GetRegisteredService<TService>()
        )();
    }

    private static Func<TService> CreateServiceConstructor<TService>(Type type)
    {
        var constructorInfo = type.GetConstructor(Type.EmptyTypes);

        if (null == constructorInfo)
        {
            throw new NullReferenceException($"Type '{type}' has no empty constructor");
        }

        return Expression.Lambda<Func<TService>>(Expression.New(constructorInfo)).Compile();
    }
}

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

        var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        ConstructorInfo? constructorInfo = null;

        foreach (var c in constructors)
        {
            constructorInfo = c;
            break;
        }

        if (null == constructorInfo)
        {
            throw new NullReferenceException($"Type '{type}' has no public empty constructor.");
        }

        var parameters = new List<(string?, object)>();
        foreach (var paramInfo in constructorInfo.GetParameters())
        {
            parameters.Add((paramInfo.Name, Resolve<object>(paramInfo.ParameterType)));
        }

        return CreateService<TService>(constructorInfo, parameters);
    }

    private static TService CreateService<TService>(ConstructorInfo constructorInfo, List<(string?, object)> parameters)
    {
        var lamdaParameterExpressions = parameters.Select((param, index) => Expression.Parameter(param.Item2.GetType(), param.Item1));
        var constructorParameterExpressions =
            lamdaParameterExpressions
                .Take(parameters.Count)
                .ToArray();

        var expression = DynamicExpression.Lambda(
            DynamicExpression.New(constructorInfo, constructorParameterExpressions),
            lamdaParameterExpressions
        ).Compile();

        // todo fix constructor with params

        var obj = expression.DynamicInvoke([.. parameters]);

        if (null == obj)
        {
            throw new NullReferenceException($"Could not construct object of type '{typeof(TService)}'.");
        }

        return (TService) obj;
    }
}

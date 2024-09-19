using System.Linq.Expressions;
using System.Reflection;

namespace IoCContainer;

public class IoCContainer
{
    private readonly Dictionary<Type, Type> _registeredServices = [];

    public void RegisterService<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _registeredServices.Add(
            typeof(TService),
            typeof(TImplementation)
        );
    }

    public void RegisterService<TService>()
        where TService : class
    {
        _registeredServices.Add(
            typeof(TService),
            typeof(TService)
        );
    }

    public Type GetRegisteredService<TService>()
        where TService : class
    {
        return GetRegisteredService<TService>(typeof(TService));
    }

    public Type GetRegisteredService<TService>(Type serviceType)
        where TService : class
    {
        if (!_registeredServices.TryGetValue(serviceType, out Type? implementationType))
        {
            throw new NullReferenceException($"Cannot resolve type '{serviceType}'. Did you forget to register it?");
        }

        return implementationType;
    }

    public TService Resolve<TService>(Type? serviceType = null)
        where TService : class
    {
        var chosenCtor = FindConstructorWithFirstOccuringEmptyChildConstructor<TService>(serviceType);

        var parameters = chosenCtor.GetParameters().Select(
            param => Resolve<object>(param.ParameterType)
        );

        return (TService) chosenCtor.Invoke([.. parameters]);
    }

    private ConstructorInfo FindConstructorWithFirstOccuringEmptyChildConstructor<TService>(Type? serviceType = null)
        where TService : class
    {
        var implementationType = GetRegisteredService<TService>(serviceType ?? typeof(TService));
        var ctors = implementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        ConstructorInfo? chosenCtor = null;

        List<CtorNestedInfo> ctorsToSearch = ctors.Select(ctor => new CtorNestedInfo(
            ctor,
            ctor.GetParameters(),
            ctor.GetParameters().Length,
            implementationType
        )).ToList();

        Type typeOfLastParam = implementationType;

        do
        {
            if (0 == ctorsToSearch.Count)
            {
                throw new NullReferenceException($"Type '{typeOfLastParam}' has no public constructor that can be resolved. Did you forget to register any of its dependencies?");
            }

            List<CtorNestedInfo> newCtorsToSearch = [];
            IEnumerable<CtorNestedInfo> sortedCtors = ctorsToSearch.OrderBy(
                ctorToSearch => ctorToSearch.TotalRequiredResolves
            );

            foreach(var ctorToSearch in sortedCtors)
            {
                if (0 == ctorToSearch.ParamsToSearch.Length)
                {
                    chosenCtor = ctorToSearch.BaseCtor;
                    break;
                }

                foreach (var paramToSearch in ctorToSearch.ParamsToSearch)
                {
                    try
                    {
                        var paramImplementationType = GetRegisteredService<object>(paramToSearch.ParameterType);
                        var paramCtors = paramImplementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

                        newCtorsToSearch.AddRange(paramCtors.Select(ctor => new CtorNestedInfo(
                            ctorToSearch.BaseCtor,
                            ctor.GetParameters(),
                            ctorToSearch.TotalRequiredResolves + ctor.GetParameters().Length,
                            paramImplementationType
                        )));
                    }
                    catch (NullReferenceException) {}
                }

                typeOfLastParam = ctorToSearch.ReturnType;
            }

            ctorsToSearch = newCtorsToSearch;
        }
        while (null == chosenCtor);

        return chosenCtor;
    }
}

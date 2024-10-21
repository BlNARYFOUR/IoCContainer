using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IoCContainer;

public class IoCContainer
{
    private readonly Dictionary<Type, List<Type>> _registeredServices = [];

    public void RegisterService<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        // todo

        _registeredServices.Add(
            typeof(TService),
            [typeof(TImplementation)]
        );
    }

    public void RegisterService<TService>()
        where TService : class
    {
        // todo

        _registeredServices.Add(
            typeof(TService),
            [typeof(TService)]
        );
    }

    // todo: add register for List type

    public List<Type> GetRegisteredServices<TService>()
        where TService : class
    {
        return GetRegisteredServices(typeof(TService));
    }

    public List<Type> GetRegisteredServices(Type serviceType)
    {
        if (!_registeredServices.TryGetValue(serviceType, out List<Type>? implementationTypes) || 0 == implementationTypes.Count)
        {
            throw new NullReferenceException($"Cannot resolve type '{serviceType}'. Did you forget to register it?");
        }

        return implementationTypes;
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
        var implementationTypes = GetRegisteredServices(serviceType ?? typeof(TService));

        var ctors = new List<ConstructorInfo>();
        var ctorsToSearch = new List<CtorNestedInfo>();
        ConstructorInfo? chosenCtor = null;

        implementationTypes.ForEach(implementationType => {
            ctors.AddRange(implementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public));

            ctorsToSearch.AddRange(
                ctors.Select(
                    ctor => new CtorNestedInfo(
                        ctor,
                        ctor.GetParameters(),
                        ctor.GetParameters().Length,
                        implementationType
                    )
                )
            );
        });

        Type typeOfLastParam = implementationTypes.ElementAt(0);

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
                        var paramImplementationType = GetRegisteredServices(paramToSearch.ParameterType);
                        var paramCtors = paramImplementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                        var referenceChain = new Dictionary<Type, bool>(ctorToSearch.ReferenceChain) {
                            { ctorToSearch.ReturnType, true },
                        };

                        if (referenceChain.ContainsKey(paramImplementationType))
                        {
                            throw new ArgumentException($"Circular reference found to type '{paramImplementationType}'. In class: '{ctorToSearch.ReturnType}'.");
                        }

                        newCtorsToSearch.AddRange(paramCtors.Select(ctor => new CtorNestedInfo(
                            ctorToSearch.BaseCtor,
                            ctor.GetParameters(),
                            ctorToSearch.TotalRequiredResolves + ctor.GetParameters().Length,
                            paramImplementationType,
                            referenceChain
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

using IoCContainer.Tests.Mocks.AbstractedServices;
using IoCContainer.Tests.Mocks.BaseClassedServices;
using IoCContainer.Tests.Mocks.CircularReferenceServices;
using IoCContainer.Tests.Mocks.CtorParamsServices;
using IoCContainer.Tests.Mocks.InterfacedServices;
using IoCContainer.Tests.Mocks.MultipleCtorServices;
using IoCContainer.Tests.Mocks.Services;

namespace IoCContainer.Tests;

public class IoCContainerTest
{
    private readonly IoCContainer _container;

    public IoCContainerTest()
    {
        _container = new IoCContainer();

        _container.RegisterService<Service>();
        _container.RegisterService<IInterfacedService, InterfacedService>();
        _container.RegisterService<BBaseClassedService, BaseClassedService>();
        _container.RegisterService<AAbstractedService, AbstractedService>();
        _container.RegisterService<CtorParamsServiceOne>();
        _container.RegisterService<ICtorParamsServiceTwo, CtorParamsServiceTwo>();
        _container.RegisterService<CtorParamsServiceThree>();
        _container.RegisterService<MultipleCtorServiceOne>();
        _container.RegisterService<MultipleCtorServiceTwo>();
        _container.RegisterService<MultipleCtorServiceThree>();
        _container.RegisterService<MultipleCtorServiceUnresolvable>();
        _container.RegisterService<MultipleCtorServiceUnresolvableTwo>();
        _container.RegisterService<MultipleCtorServiceUnresolvableThree>();
        _container.RegisterService<MultipleCtorServiceUnresolvableFour>();
        _container.RegisterService<CircularReferenceServiceOne>();
        _container.RegisterService<CircularReferenceServiceTwo>();
        _container.RegisterService<CircularReferenceServiceThree>();
    }

    [Fact]
    public void ItCanGetARegisteredServiceTest()
    {
        Type service = _container.GetRegisteredServices<Service>();
        Assert.Equal(typeof(Service), service);
    }

    [Fact]
    public void ItCanGetARegisteredInterfacedServiceTest()
    {
        Type service = _container.GetRegisteredServices<Service>();
        Assert.Equal(typeof(Service), service);
    }

    [Fact]
    public void ItCanGetARegisteredBaseClassedServiceTest()
    {
        Type service = _container.GetRegisteredServices<BBaseClassedService>();
        Assert.Equal(typeof(BaseClassedService), service);
    }

    [Fact]
    public void ItCanGetARegisteredAbstractedServiceTest()
    {
        Type service = _container.GetRegisteredServices<AAbstractedService>();
        Assert.Equal(typeof(AbstractedService), service);
    }

    [Fact]
    public void ItCannotResolveAnUnregisteredServiceTest()
    {
        var exception = Assert.ThrowsAny<Exception>(() => {
            _container.Resolve<UnresolvedService>();
        });

        Assert.Equal($"Cannot resolve type '{typeof(UnresolvedService)}'. Did you forget to register it?", exception.Message);
    }

    [Fact]
    public void ItCanResolveAServiceTest()
    {
        var result = _container.Resolve<Service>();

        Assert.IsType<Service>(result);
        Assert.Equal(1, result.GetInt());
    }

    [Fact]
    public void ItCanResolveAnInterfacedServiceTest()
    {
        var result = _container.Resolve<IInterfacedService>();

        Assert.IsType<InterfacedService>(result);
        Assert.Equal(1, result.GetInt());
    }

    [Fact]
    public void ItCanResolveABaseClassedServiceTest()
    {
        var result = _container.Resolve<BBaseClassedService>();

        Assert.IsType<BaseClassedService>(result);
        Assert.Equal(1, result.GetInt());
    }

    [Fact]
    public void ItCanResolveAnAbstractedServiceTest()
    {
        var result = _container.Resolve<BBaseClassedService>();

        Assert.IsType<BaseClassedService>(result);
        Assert.Equal(1, result.GetInt());
    }

    [Fact]
    public void ItCanResolveAServiceWithDependenciesTest()
    {
        var result = _container.Resolve<CtorParamsServiceOne>();

        Assert.IsType<CtorParamsServiceOne>(result);
        Assert.Equal(2, result.GetInt());
    }

    [Fact]
    public void ItCanResolveAServiceWithNestedDependenciesTest()
    {
        var result = _container.Resolve<CtorParamsServiceThree>();

        Assert.IsType<CtorParamsServiceThree>(result);
        Assert.Equal(6, result.GetInt());
    }

    [Fact]
    public void ItCanResolveAServiceWithMultipleConstructorsTest()
    {
        var result = _container.Resolve<MultipleCtorServiceOne>();

        Assert.IsType<MultipleCtorServiceOne>(result);
        Assert.Equal(2, result.GetInt());
    }

    [Fact]
    public void ItCanResolveNestedServicesWithMultipleConstructorsTest()
    {
        var result = _container.Resolve<MultipleCtorServiceTwo>();

        Assert.IsType<MultipleCtorServiceTwo>(result);
        Assert.Equal(4, result.GetInt());
    }

    [Fact]
    public void ItCanResolveMultiNestedServicesWithMultipleConstructorsTest()
    {
        var result = _container.Resolve<MultipleCtorServiceThree>();

        Assert.IsType<MultipleCtorServiceThree>(result);
        Assert.Equal(5, result.GetInt());
    }

    [Fact]
    public void ItCannotResolveAServiceWithADependencyOnAServiceWithoutSomeRegisteredServicesConstructorTest()
    {
        var exception = Assert.ThrowsAny<Exception>(() => {
            _container.Resolve<MultipleCtorServiceUnresolvableTwo>();
        });

        Assert.Equal($"Cannot resolve type '{typeof(UnresolvedService)}'. Did you forget to register it?", exception.Message);
    }

    [Fact]
    public void ItCannotResolveAServiceWithADependencyOnAServiceWithoutAnyRegisteredServicesConstructorTest()
    {
        var exception = Assert.ThrowsAny<Exception>(() => {
            _container.Resolve<MultipleCtorServiceUnresolvableFour>();
        });

        Assert.Equal($"Type '{typeof(MultipleCtorServiceUnresolvableThree)}' has no public constructor that can be resolved. Did you forget to register any of its dependencies?", exception.Message);
    }

    [Fact]
    public void ItCannotResolveAServiceWithACircularReferenceTest()
    {
        var exception = Assert.ThrowsAny<Exception>(() => {
            _container.Resolve<CircularReferenceServiceThree>();
        });

        Assert.Equal($"Circular reference found to type '{typeof(CircularReferenceServiceThree)}'. In class: '{typeof(CircularReferenceServiceOne)}'.", exception.Message);
    }
}

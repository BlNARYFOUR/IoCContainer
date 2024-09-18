using IoCContainer.Tests.Mocks.AbstractedService;
using IoCContainer.Tests.Mocks.BaseClassedService;
using IoCContainer.Tests.Mocks.InterfacedService;
using IoCContainer.Tests.Mocks.Service;

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
    }

    [Fact]
    public void ItCanGetARegisteredServiceTest()
    {
        Type service = _container.GetRegisteredService<Service>();
        Assert.Equal(typeof(Service), service);
    }

    [Fact]
    public void ItCanGetARegisteredInterfacedServiceTest()
    {
        Type service = _container.GetRegisteredService<Service>();
        Assert.Equal(typeof(Service), service);
    }

    [Fact]
    public void ItCanGetARegisteredBaseClassedServiceTest()
    {
        Type service = _container.GetRegisteredService<BBaseClassedService>();
        Assert.Equal(typeof(BaseClassedService), service);
    }

    [Fact]
    public void ItCanGetARegisteredAbstractedServiceTest()
    {
        Type service = _container.GetRegisteredService<AAbstractedService>();
        Assert.Equal(typeof(AbstractedService), service);
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
        Assert.Equal(1, result.GetInt());
    }
}

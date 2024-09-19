using IoCContainer.Tests.Mocks.AbstractedServices;
using IoCContainer.Tests.Mocks.BaseClassedServices;
using IoCContainer.Tests.Mocks.CtorParamsServices;
using IoCContainer.Tests.Mocks.InterfacedServices;
using IoCContainer.Tests.Mocks.Services;

namespace IoCContainer.Tests.Mocks.MultipleCtorServices;

internal class MultipleCtorServiceTwo
{
    private readonly Service _service;
    private readonly IInterfacedService _interfacedService;
    private readonly AAbstractedService _abstractedService;
    private readonly BBaseClassedService? _baseClassedService;
    private readonly CtorParamsServiceOne? _ctorParamsServiceOne;

    // 4 total resolves
    public MultipleCtorServiceTwo(
        Service service,
        IInterfacedService interfacedService,
        AAbstractedService abstractedService,
        BBaseClassedService baseClassedService
    )
    {
        _service = service;
        _interfacedService = interfacedService;
        _abstractedService = abstractedService;
        _baseClassedService = baseClassedService;
        _ctorParamsServiceOne = null;
    }

    // 5 total resolves
    public MultipleCtorServiceTwo(
        Service service,
        IInterfacedService interfacedService,
        AAbstractedService abstractedService,
        CtorParamsServiceOne ctorParamsServiceOne
    )
    {
        _service = service;
        _interfacedService = interfacedService;
        _abstractedService = abstractedService;
        _baseClassedService = null;
        _ctorParamsServiceOne = ctorParamsServiceOne;
    }

    // should return 4
    public int GetInt() => _service.GetInt()
        + _interfacedService.GetInt()
        + _abstractedService.GetInt()
        + (_baseClassedService?.GetInt() ?? 0)
        + (_ctorParamsServiceOne?.GetInt() ?? 0);
}

using IoCContainer.Tests.Mocks.Services;
using IoCContainer.Tests.Mocks.InterfacedServices;
using IoCContainer.Tests.Mocks.AbstractedServices;

namespace IoCContainer.Tests.Mocks.MultipleCtorServices;

internal class MultipleCtorServiceOne
{
    private readonly Service _service;
    private readonly IInterfacedService _interfacedService;
    private readonly AAbstractedService? _abstractedService;

    public MultipleCtorServiceOne(
        Service service,
        IInterfacedService interfacedService,
        AAbstractedService abstractedService
    )
    {
        _service = service;
        _interfacedService = interfacedService;
        _abstractedService = abstractedService;
    }

    public MultipleCtorServiceOne(
        Service service,
        IInterfacedService interfacedService
    ) {
        _service = service;
        _interfacedService = interfacedService;
        _abstractedService = null;
    }

    // should return 2
    public int GetInt() => _service.GetInt() + _interfacedService.GetInt() + (_abstractedService?.GetInt() ?? 0);
}

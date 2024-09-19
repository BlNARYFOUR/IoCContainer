using IoCContainer.Tests.Mocks.Services;
using IoCContainer.Tests.Mocks.InterfacedServices;

namespace IoCContainer.Tests.Mocks.CtorParamsServices;

internal class CtorParamsServiceOne(
    Service service,
    IInterfacedService interfacedService
) {
    private readonly Service _service = service;
    private readonly IInterfacedService _interfacedService = interfacedService;

    // should return 2
    public int GetInt() => _service.GetInt() + _interfacedService.GetInt();
}

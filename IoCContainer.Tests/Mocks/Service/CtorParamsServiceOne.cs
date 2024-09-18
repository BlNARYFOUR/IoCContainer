using IoCContainer.Tests.Mocks.InterfacedService;

namespace IoCContainer.Tests.Mocks.Service;

internal class CtorParamsServiceOne(
    Service service,
    IInterfacedService interfacedService
)
{
    private Service _service = service;
    private IInterfacedService _interfacedService = interfacedService;

    public int GetInt() => _service.GetInt() + _interfacedService.GetInt();
}

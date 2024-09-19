using IoCContainer.Tests.Mocks.Services;

namespace IoCContainer.Tests.Mocks.CtorParamsServices;

internal class CtorParamsServiceTwo(
    Service service,
    CtorParamsServiceOne ctorParamsServiceOne
) : ICtorParamsServiceTwo {
    private readonly Service _service = service;
    private readonly CtorParamsServiceOne _ctorParamsServiceOne = ctorParamsServiceOne;

    // should return 3
    public int GetInt() => _ctorParamsServiceOne.GetInt() + _service.GetInt();
}

using IoCContainer.Tests.Mocks.Services;

namespace IoCContainer.Tests.Mocks.CtorParamsServices;

internal class CtorParamsServiceThree(
    Service service,
    ICtorParamsServiceTwo ctorParamsServiceTwo,
    CtorParamsServiceOne ctorParamsServiceOne
) {
    private readonly Service _service = service;
    private readonly CtorParamsServiceOne _ctorParamsServiceOne = ctorParamsServiceOne;
    private readonly ICtorParamsServiceTwo _ctorParamsServiceTwo = ctorParamsServiceTwo;

    // should return 6
    public int GetInt() => _ctorParamsServiceOne.GetInt() + _ctorParamsServiceTwo.GetInt() + _service.GetInt();
}

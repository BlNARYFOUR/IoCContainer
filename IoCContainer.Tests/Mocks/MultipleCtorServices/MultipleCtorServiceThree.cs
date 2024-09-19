using IoCContainer.Tests.Mocks.CtorParamsServices;
using IoCContainer.Tests.Mocks.InterfacedServices;
using IoCContainer.Tests.Mocks.Services;

namespace IoCContainer.Tests.Mocks.MultipleCtorServices;

internal class MultipleCtorServiceThree
{
    private readonly UnresolvedService? _unresolvedService;
    private readonly CtorParamsServiceThree? _ctorParamsServiceThree;
    private readonly MultipleCtorServiceTwo? _multipleCtorServiceTwo;
    private readonly IInterfacedService? _interfacedService;

    // 1 resolve (but unknown!)
    public MultipleCtorServiceThree(
        UnresolvedService unresolvedService
    ) {
        _unresolvedService = unresolvedService;
        _ctorParamsServiceThree = null;
        _multipleCtorServiceTwo = null;
        _interfacedService = null;
    }

    // 6 total resolves
    public MultipleCtorServiceThree(
        CtorParamsServiceThree ctorParamsServiceThree
    ) {
        _unresolvedService = null;
        _ctorParamsServiceThree = ctorParamsServiceThree;
        _multipleCtorServiceTwo = null;
        _interfacedService = null;
    }

    // 5 total resolves
    public MultipleCtorServiceThree(
        MultipleCtorServiceTwo multipleCtorServiceTwo,
        IInterfacedService interfacedService
    )
    {
        _unresolvedService = null;
        _ctorParamsServiceThree = null;
        _multipleCtorServiceTwo = multipleCtorServiceTwo;
        _interfacedService = interfacedService;
    }

    // should return 5
    public int GetInt() => (_unresolvedService?.GetInt() ?? 0)
        + (_ctorParamsServiceThree?.GetInt() ?? 0)
        + (_multipleCtorServiceTwo?.GetInt() ?? 0)
        + (_interfacedService?.GetInt() ?? 0);
}

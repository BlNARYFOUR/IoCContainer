using IoCContainer.Tests.Mocks.Services;

namespace IoCContainer.Tests.Mocks.MultipleCtorServices;

internal class MultipleCtorServiceUnresolvable
{
    public MultipleCtorServiceUnresolvable(
#pragma warning disable IDE0060 // Remove unused parameter
        Service service,
        UnresolvedService unresolvedService
    ) {   
    }

    public MultipleCtorServiceUnresolvable(
        UnresolvedService unresolvedService
#pragma warning restore IDE0060 // Remove unused parameter
    ) {
    }
}

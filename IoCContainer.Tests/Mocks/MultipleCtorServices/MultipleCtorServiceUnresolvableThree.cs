using IoCContainer.Tests.Mocks.Services;

namespace IoCContainer.Tests.Mocks.MultipleCtorServices;

internal class MultipleCtorServiceUnresolvableThree
{
#pragma warning disable IDE0290 // Use primary constructor
    public MultipleCtorServiceUnresolvableThree(
#pragma warning restore IDE0290 // Use primary constructor
#pragma warning disable IDE0060 // Remove unused parameter
        UnresolvedService unresolvedService
#pragma warning restore IDE0060 // Remove unused parameter
    ) {
    }
}

namespace IoCContainer.Tests.Mocks.MultipleCtorServices;

internal class MultipleCtorServiceUnresolvableFour(
#pragma warning disable CS9113 // Parameter is unread.
    MultipleCtorServiceUnresolvableThree service
#pragma warning restore CS9113 // Parameter is unread.
) {
}

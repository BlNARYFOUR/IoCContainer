namespace IoCContainer.Tests.Mocks.MultipleCtorServices;

internal class MultipleCtorServiceUnresolvableTwo(
#pragma warning disable CS9113 // Parameter is unread.
    MultipleCtorServiceUnresolvable service
#pragma warning restore CS9113 // Parameter is unread.
) {
}

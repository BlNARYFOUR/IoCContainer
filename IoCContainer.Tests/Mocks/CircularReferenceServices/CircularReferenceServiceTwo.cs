namespace IoCContainer.Tests.Mocks.CircularReferenceServices;

internal class CircularReferenceServiceTwo
{
    public CircularReferenceServiceTwo(
#pragma warning disable IDE0060 // Remove unused parameter
        CircularReferenceServiceOne service
#pragma warning restore CS9113 // Parameter is unread.
    ) {
    }
}

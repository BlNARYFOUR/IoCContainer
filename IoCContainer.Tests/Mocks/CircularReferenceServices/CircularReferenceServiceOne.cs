namespace IoCContainer.Tests.Mocks.CircularReferenceServices;

internal class CircularReferenceServiceOne
{
    public CircularReferenceServiceOne(
#pragma warning disable IDE0060 // Remove unused parameter
        CircularReferenceServiceThree service
#pragma warning restore CS9113 // Parameter is unread.
    ) {
    }
}

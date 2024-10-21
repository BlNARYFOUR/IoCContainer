namespace IoCContainer.Tests.Mocks.CircularReferenceServices;

internal class CircularReferenceServiceThree
{
    public CircularReferenceServiceThree(
#pragma warning disable IDE0060 // Remove unused parameter
        CircularReferenceServiceTwo service
#pragma warning restore CS9113 // Parameter is unread.
    ) {
    }
}

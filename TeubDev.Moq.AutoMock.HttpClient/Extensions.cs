using Moq.AutoMock;

namespace TeubDev.Moq.AutoMock;

/// <summary>
/// Contains extension methods.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Gets an <see cref="HttpClientMock"/> for use in unit tests.
    /// </summary>
    public static HttpClientMock GetHttpClientMock(this AutoMocker autoMocker) => new (autoMocker);
}

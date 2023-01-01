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
    /// <param name="autoMocker">The AutoMocker used for getting mocks.</param>
    /// <param name="baseAddress">The base address that should be passed into a created <see cref="HttpClient"/></param>
    /// <remarks>
    /// The baseAddress parameter is needed if the unit test uses a relative URI instead of absolute.
    /// </remarks>
    public static HttpClientMock GetHttpClientMock(this AutoMocker autoMocker, Uri? baseAddress = null) => new (autoMocker, baseAddress);
}

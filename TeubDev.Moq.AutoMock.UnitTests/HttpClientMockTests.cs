using Moq;
using Moq.AutoMock;

namespace TeubDev.Moq.AutoMock.UnitTests;

public class HttpClientMockTests
{
    [Test]
    public async Task SetupAndVerify_MatchingCall_NoException()
    {
        var mocker = new AutoMocker();

        var url = "stuff";
        var baseAddress = new Uri("https://google.com/");
        var expectedResponse = new HttpResponseMessage
        {
            Content = new StringContent("This is the response"),
            StatusCode = System.Net.HttpStatusCode.OK,
        };
        var mockClient = mocker.GetHttpClientMock(baseAddress);
        mockClient
            .SetupRequest(m =>
                m.RequestUri == new Uri(baseAddress, url)
                && m.Method == HttpMethod.Get)
            .ReturnsAsync(expectedResponse);
        var client = mocker.Get<HttpClient>();

        using var result = await client.GetAsync(url);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(expectedResponse));
            mockClient.Verify(m =>
                m.RequestUri == new Uri(baseAddress, url)
                && m.Method == HttpMethod.Get, Times.Once);
        });
    }

    [Test]
    public async Task SetupAndVerify_NonmatchingVerify_ThrowsMockException()
    {
        var mocker = new AutoMocker();
        var url = "stuff";
        var baseAddress = new Uri("https://google.com/");
        var expectedResponse = new HttpResponseMessage
        {
            Content = new StringContent("This is the response"),
            StatusCode = System.Net.HttpStatusCode.OK,
        };
        var mockClient = mocker.GetHttpClientMock(baseAddress);
        mockClient
            .SetupRequest(m =>
                m.RequestUri == new Uri(baseAddress, url)
                && m.Method == HttpMethod.Get)
            .ReturnsAsync(expectedResponse);
        var client = mocker.Get<HttpClient>();

        using var result = await client.GetAsync(url);

        Assert.Throws<MockException>(() =>
        {
            mockClient.Verify(m =>
                m.RequestUri == new Uri(baseAddress, url + "thisWillNotMatch")
                && m.Method == HttpMethod.Get, Times.Once);
        });
    }
}

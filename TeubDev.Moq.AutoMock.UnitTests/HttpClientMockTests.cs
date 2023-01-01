using Moq;
using Moq.AutoMock;

namespace TeubDev.Moq.AutoMock.UnitTests;

public class HttpClientMockTests
{
    [Test]
    public async Task SetupAndVerify_MatchingCall_NoException()
    {
        var mocker = new AutoMocker();
        var mockClient = mocker.GetHttpClientMock();
        var url = "stuff";
        var baseUri = new Uri("https://google.com/");
        var expectedResponse = new HttpResponseMessage
        {
            Content = new StringContent("This is the response"),
            StatusCode = System.Net.HttpStatusCode.OK,
        };
        mockClient
            .SetupRequest(m =>
                m.RequestUri == new Uri(baseUri, url)
                && m.Method == HttpMethod.Get)
            .ReturnsAsync(expectedResponse);
        var client = mocker.CreateInstance<HttpClient>();
        client.BaseAddress = baseUri;

        using var result = await client.GetAsync(url);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(expectedResponse));
            mockClient.Verify(m =>
                (m.RequestUri == new Uri(baseUri, url)
                && m.Method == HttpMethod.Get), Times.Once);
        });
    }

    [Test]
    public async Task SetupAndVerify_NonmatchingVerify_ThrowsMockException()
    {
        var mocker = new AutoMocker();
        var mockClient = mocker.GetHttpClientMock();
        var url = "stuff";
        var baseUri = new Uri("https://google.com/");
        var expectedResponse = new HttpResponseMessage
        {
            Content = new StringContent("This is the response"),
            StatusCode = System.Net.HttpStatusCode.OK,
        };
        mockClient
            .SetupRequest(m =>
                m.RequestUri == new Uri(baseUri, url)
                && m.Method == HttpMethod.Get)
            .ReturnsAsync(expectedResponse);
        var client = mocker.CreateInstance<HttpClient>();
        client.BaseAddress = baseUri;

        using var result = await client.GetAsync(url);

        Assert.Throws<MockException>(() =>
        {
            mockClient.Verify(m =>
                (m.RequestUri == new Uri(baseUri, url + "thisWillNotMatch")
                && m.Method == HttpMethod.Get), Times.Once);
        });
    }
}

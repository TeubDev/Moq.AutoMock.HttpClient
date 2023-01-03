using Moq;
using Moq.AutoMock;
using Moq.Language.Flow;
using Moq.Protected;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TeubDev.Moq.AutoMock
{
    /// <summary>
    /// A fake little wrapper that makes it a bit easier to mock out HttpClients.
    /// </summary>
    public class HttpClientMock
    {
        private readonly Mock<HttpMessageHandler> handlerMock;

        internal HttpClientMock(AutoMocker mocker, Uri? baseAddress)
        {
            handlerMock = mocker.GetMock<HttpMessageHandler>();
            var httpClient = mocker.CreateInstance<HttpClient>();
            if (baseAddress != null)
            {
                httpClient.BaseAddress = baseAddress;
            }
            mocker.Use(httpClient);
        }

        /// <summary>
        /// Specifies a setup for an invocation on the SendAsync method of an underlying HttpClientHandler
        /// </summary>
        /// <param name="requestMatcher">A function for things to check about a request to verify it matches the setup.</param>
        public ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupRequest(
            Expression<Func<HttpRequestMessage, bool>> requestMatcher) =>
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is(requestMatcher),
                    ItExpr.IsAny<CancellationToken>());

        /// <summary>
        /// Verifies that the matching request was called the given number of times.
        /// </summary>
        public void Verify(Expression<Func<HttpRequestMessage, bool>> requestMatcher, Times times) =>
            handlerMock.Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", times, ItExpr.Is(requestMatcher),
                    ItExpr.IsAny<CancellationToken>());

        /// <inheritdoc cref="Verify(Expression{Func{HttpRequestMessage, bool}}, Times)"/>
        public void Verify(Expression<Func<HttpRequestMessage, bool>> requestMatcher, Func<Times> times) =>
            Verify(requestMatcher, times());

        /// <inheritdoc cref="Verify(Expression{Func{HttpRequestMessage, bool}}, Times)"/>
        public void Verify(Expression<Func<HttpRequestMessage, bool>> requestMatcher) =>
            Verify(requestMatcher, Times.Once());

    }
}
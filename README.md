This package has been deprecated in favor of Moq.Contrib.HttpClient, which has most of the same features plus more.

# Moq.AutoMock.HttpClient
A NuGet package for making HttpClients slightly easier to test.

This wraps and mocks the `HttpMessageHandler` class used by the `HttpClient` class and internally creates an `HttpClient` to be used by classes under test.

## Usage
### Setup
After creating an instance of the `AutoMocker` class (see [Moq.AutoMocker](https://github.com/moq/Moq.AutoMocker)), use the `GetHttpClientMock` extension method to create an instance of the `HttpClientMock` class.

```csharp
var mocker = new AutoMocker();
var mockClient = mocker.GetHttpClientMock();
```

To set up to watch for a request, use the `SetupRequest` method, passing in a function that will determine if a particular request matches the setup or not.
```csharp
var url = "https://google.com/";
mockClient
    .SetupRequest(m =>
        m.RequestUri == new Uri(url)
        && m.Method == HttpMethod.Get)
    .ReturnsAsync(new HttpResponseMessage
    {
        Content = new StringContent("This is the response"),
        StatusCode = System.Net.HttpStatusCode.OK,
    });
```

Note: If using relative URIs instead of absolute URIs (like in the example above), create base address and pass it into the optional parameter in `GetHttpClientMock`, and any request matching will need to account for both URIs.
```csharp
var url = "stuff";
var baseAddress = new Uri("https://google.com/");
var mockClient = mocker.GetHttpClientMock(baseAddress);
// ...
    m.RequestUri == new Uri(baseAddress, url)
```

### Verify
Similar to regular Moq mocks, verification is similar to setup.
```csharp
mockClient.Verify(m =>
    m.RequestUri == new Uri(url)
    && m.Method == HttpMethod.Get, Times.Once);
```

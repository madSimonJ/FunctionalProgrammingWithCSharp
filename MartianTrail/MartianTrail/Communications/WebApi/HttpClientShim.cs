namespace MartianTrail.Communications.WebApi;

public class HttpClientShim : IHttpClient
{
    private readonly HttpClient _httpClient;

    public HttpClientShim(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> GetAsync(string url) =>
        _httpClient.GetAsync(url);
}
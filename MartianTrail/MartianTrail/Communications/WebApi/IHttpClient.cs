namespace MartianTrail.Communications.WebApi;

public interface IHttpClient
{
    Task<HttpResponseMessage> GetAsync(string url);
}
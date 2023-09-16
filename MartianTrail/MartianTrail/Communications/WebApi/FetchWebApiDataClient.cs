using System.Text.Json;
using MartianTrail.Common;

namespace MartianTrail.Communications.WebApi;

public class FetchWebApiDataClient : IFetchWebApiData
{
    private readonly IHttpClient _httpClient;

    public FetchWebApiDataClient(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Maybe<T>> FetchData<T>(string url)
    {
        try
        {
            var response = await this._httpClient.GetAsync(url);
            Maybe<string> data = response.IsSuccessStatusCode
                ? new Something<string>(await response.Content.ReadAsStringAsync())
                : new Nothing<string>();

            var contentStream = await data.BindAsync(x => response.Content.ReadAsStreamAsync());
            var returnValue = await contentStream.BindAsync(x => JsonSerializer.DeserializeAsync<T>(x));
            return returnValue;
        }
        catch (Exception e)
        {
            return new Error<T>(e);
        }
    }
}
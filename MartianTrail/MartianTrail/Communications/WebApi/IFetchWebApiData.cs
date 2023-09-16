using MartianTrail.Common;

namespace MartianTrail.Communications.WebApi
{
    public interface IFetchWebApiData
    {
        Task<Maybe<T>> FetchData<T>(string url);
    }
}

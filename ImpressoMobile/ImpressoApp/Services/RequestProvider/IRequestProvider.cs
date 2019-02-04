using System.Threading.Tasks;
using Java.Util;
using System.Collections.Generic;

namespace ImpressoApp.Services.RequestProvider
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string endpoint, List<ReqestParameter> parameters = null);
        Task<TResult> PostAsync<TResult>(string endpoint, object data);
        //Task<TResult> PostAsync<TResult>(string endpoint, Dictionary<string, string> data);
        Task<TResult> PutAsync<TResult>(string endpoint, TResult data);
        Task DeleteAsync(string endpoint);
        void InitWithAuthorizationToken(string token);
    }
}

using System;
using System.Threading.Tasks;
using ImpressoApp.Constants;
using ImpressoApp.Models;
using ImpressoApp.Services.RequestProvider;
namespace ImpressoApp.Services.Connect
{
    public class ConnectService : IConnectService
    {
        private const string ConnectToPersonEndpoint = ApplicationConstants.LiveServerApi + "api/Connect/ConnectPerson";

        private readonly IRequestProvider requestProvider;

        public ConnectService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<BaseResponseModel> ConnectToPersonAsync(string userID)
        {
            return await requestProvider.PostAsync<BaseResponseModel>(ConnectToPersonEndpoint, userID);
        }
    }
}

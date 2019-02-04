using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Constants;
using ImpressoApp.Models.Token;
using ImpressoApp.Services.RequestProvider;
using FFImageLoading;

namespace ImpressoApp.Services.Token
{
    public class TokenService : ITokenService
    {
        private static readonly string ListTokensEndpoint = ApplicationConstants.LiveServerApi + "/api/Token/ListTokens/";
        private static readonly string GetTotalTokensEndpoint = ApplicationConstants.LiveServerApi + "/api/Token/GetTotalTokensCount/";

        private readonly IRequestProvider requestProvider;

        public TokenService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<TokenServerModel>> GetListTokens()
        {
            return await requestProvider.GetAsync<List<TokenServerModel>>(ListTokensEndpoint);
        }

        public async Task<double> GetTotalTokensAmount()
        {
            return await requestProvider.GetAsync<double>(GetTotalTokensEndpoint);
        }

        //public Task<List<TokenModel>> GetListTokens()
        //{
        //    return Task.Run(() =>
        //    {
        //        var resultList = new List<TokenModel>();

        //        var token1 = new TokenModel
        //        {
        //            SendReceiveTokenType = Enums.SendReceiveTokenType.Receive,
        //            Timestamp = DateTime.Now,
        //            Amount = 12345,
        //            Description = "Company X sent you 12345 tokens for viewewing your profile"
        //        };

        //        var token2 = new TokenModel
        //        {
        //            SendReceiveTokenType = Enums.SendReceiveTokenType.Send,
        //            Timestamp = DateTime.Now,
        //            Amount = 125,
        //            Description = "You sent Company X 125 tokens for viewewing their profile"
        //        };

        //        var token3 = new TokenModel
        //        {
        //            SendReceiveTokenType = Enums.SendReceiveTokenType.Receive,
        //            Timestamp = DateTime.Now,
        //            Amount = 123,
        //            Description = "Company Z sent you 123 tokens for viewewing your profile"
        //        };

        //        var token4 = new TokenModel
        //        {
        //            SendReceiveTokenType = Enums.SendReceiveTokenType.Send,
        //            Timestamp = DateTime.Now,
        //            Amount = 567,
        //            Description = "You sent Company X 567 tokens for viewewing their profile"
        //        };

        //        var token5 = new TokenModel
        //        {
        //            SendReceiveTokenType = Enums.SendReceiveTokenType.Receive,
        //            Timestamp = DateTime.Now,
        //            Amount = 123,
        //            Description = "Company Z sent you 123 tokens for viewewing your profile"
        //        };

        //        var token6 = new TokenModel
        //        {
        //            SendReceiveTokenType = Enums.SendReceiveTokenType.Send,
        //            Timestamp = DateTime.Now,
        //            Amount = 567,
        //            Description = "You sent Company X 567 tokens for viewewing their profile"
        //        };

        //        resultList.Add(token1);
        //        resultList.Add(token2);
        //        resultList.Add(token3);
        //        resultList.Add(token4);
        //        resultList.Add(token5);
        //        resultList.Add(token6);

        //        return resultList;
        //    });

        //}

        //public Task<double> GetTotalTokensAmount()
        //{
        //    return Task.Run(() => { return 1234.34; });
        //}

    }
}

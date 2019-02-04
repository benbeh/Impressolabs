using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ImpressoApp.Models.Token;
namespace ImpressoApp.Services.Token
{
    public interface ITokenService
    {
        Task<List<TokenServerModel>> GetListTokens();

        Task<double> GetTotalTokensAmount();
    }
}

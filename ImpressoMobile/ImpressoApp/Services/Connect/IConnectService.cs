using System;
using System.Threading.Tasks;
using ImpressoApp.Models;
namespace ImpressoApp.Services.Connect
{
    public interface IConnectService
    {
        Task<BaseResponseModel> ConnectToPersonAsync(string userID);
    }
}

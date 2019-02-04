using System;
using System.Threading.Tasks;
using ImpressoApp.Models.Authentication;
using ImpressoApp.Models;

namespace ImpressoApp.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<LoginResponseModel> LoginAsync(string email, string password, string token = "");

        Task<string> TokenLoginAsync(string email, string token);

        Task<BaseResponseModel> SignUpAsync(string firstName,
                                            string lastName,
                                            string companyName,
                                            string email,
                                            string password,
                                            string confirmPassword,
                                            int role,
                                            string base64Img);

        Task<LoginResponseModel> LoginFacebookAsync(string token);

        Task<bool> GetIsCompany();
    }
}

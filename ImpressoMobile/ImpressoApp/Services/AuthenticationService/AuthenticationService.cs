using System.Threading.Tasks;
using ImpressoApp.Services.RequestProvider;
using ImpressoApp.Constants;
using ImpressoApp.Models.Authentication;
using ImpressoApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace ImpressoApp.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string LoginEndpoint = ApplicationConstants.LiveServerApi + "api/Account/Login/";
        private const string SignUpEndpoint = ApplicationConstants.LiveServerApi + "api/Account/Register/";
        private const string LoginFacebookEndpoint = ApplicationConstants.LiveServerApi + "api/Account/LoginFacebook/";
        private const string ParseJwtEndpoint = ApplicationConstants.LiveServerApi + "/api/User/GetInformationFromJWT/claims/";

        private IRequestProvider requestProvider;

        public AuthenticationService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<LoginResponseModel> LoginAsync(string email, string password, string token = "")
        {
            var model = new LoginModel { Email = email, Password = password };
            var result = await requestProvider.PostAsync<LoginResponseModel>(LoginEndpoint, model);
            return result;
        }

        public async Task<bool> GetIsCompany()
        {
            var jwt = await requestProvider.GetAsync<List<JWTEntity>>(ParseJwtEndpoint);
            return jwt.FirstOrDefault((entity) => entity.type.Contains("role") && entity.value == "HiringManager") != null;
        }

        public async Task<string> TokenLoginAsync(string email, string token)
        {
            return await requestProvider.PostAsync<string>(LoginEndpoint, email);
        }

        public async Task<BaseResponseModel> SignUpAsync(string firstName, string lastName, string companyName, string email, string password, string confirmPassword, int role, string base64Img)
        {
            var requestModel = new SignUpRequestModel
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                CompanyName = companyName,
                Password = password,
                ConfirmPassword = confirmPassword,
                Role = role,
                Base64Img = base64Img
            };

            return await requestProvider.PostAsync<BaseResponseModel>(SignUpEndpoint, requestModel);
        }

        public async Task<LoginResponseModel> LoginFacebookAsync(string token)
        {
            return await requestProvider.PostAsync<LoginResponseModel>(LoginFacebookEndpoint, new Dictionary<string, string>() { ["accessToken"] = token } );
        }

        public class JWTEntity
        {
            public string type { get; set; }
            public string value { get; set; }

        }
    }
}

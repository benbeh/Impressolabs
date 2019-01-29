using System;
using System.Threading.Tasks;
using ImpressoApp.Models.User;
using ImpressoApp.Models;

namespace ImpressoApp.Services.User
{
    public interface IUserService
    {
        Task<UserNameWithPhotoModel> GetUserNameWithPhoto();

        Task<BaseResponseModel> SetAsBookmarkedAsync(SetAsBookmarkedRequestModel model);

        Task<UserModel> GetUserById(string id);

        Task<BaseResponseModel> EditUserProfileAsync(UserModel userModel);

        Task<CompanyInfoModel> GetCurrentCompanyInfo();

        Task<BaseResponseModel> SetEthereumAddressAsync(string address);

        Task<string> GetCurrentUserEthereumAddressAsync();
    }
}

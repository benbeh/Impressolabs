using System;
using ImpressoApp.Constants;
using ImpressoApp.Services.RequestProvider;
using System.Threading.Tasks;
using ImpressoApp.Models.User;
using ImpressoApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImpressoApp.Models.Project;

namespace ImpressoApp.Services.User
{
    public class UserService : IUserService
    {
        private const string UserNameWithPhotoEndpoint = ApplicationConstants.LiveServerApi + "api/User/GetCurrentUserNameWithPhoto";
        private const string SetAsBookmarkedEndpoint = ApplicationConstants.LiveServerApi + "api/User/SetAsBookmarked";
        private const string GetUserProfileInfoByIdEndpoint = ApplicationConstants.LiveServerApi + "api/User/GetUserProfileInfoById";
        private const string EditUserProfleEndpoint = ApplicationConstants.LiveServerApi + "api/User/EditUserProfile";
        private const string GetCurrentCompanyInfoEndpoint = ApplicationConstants.LiveServerApi + "api/Company/GetCompanyProfile";
        private const string ConnectEndpoint = ApplicationConstants.LiveServerApi + "/api/Connect/ConnectPerson";
        private const string SetEtherumAddressEndpoint = ApplicationConstants.LiveServerApi + "/api/User/SetEthereumAddress";

        private const string CreateProjectEndpoint = ApplicationConstants.LiveServerApi + "/api/Project/CreateProject/";

        private readonly IRequestProvider requestProvider;

        public UserService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public Task<UserModel> GetUserById(string id)
        {
            return requestProvider.GetAsync<UserModel>(GetUserProfileInfoByIdEndpoint, new List<ReqestParameter> { new ReqestParameter() { Name = "userId", Value = id } });
        }

        public async Task<BaseResponseModel> EditUserProfileAsync(UserModel userModel)
        {
            return await requestProvider.PostAsync<BaseResponseModel>(EditUserProfleEndpoint, userModel);
        }

        public async Task<BaseResponseModel> SetEthereumAddressAsync(string address)
        {
            return await requestProvider.PostAsync<BaseResponseModel>(SetEtherumAddressEndpoint, address);
        }

        public Task<UserNameWithPhotoModel> GetUserNameWithPhoto()
        {
            return requestProvider.GetAsync<UserNameWithPhotoModel>(UserNameWithPhotoEndpoint);
        }

        public async Task<string> GetCurrentUserEthereumAddressAsync()
        {
            return (await GetUserById(Settings.UserID)).EthereumAddress;
        }

        public Task Connect(string id)
        {
            return requestProvider.GetAsync<BaseResponseModel>(ConnectEndpoint);
        }

        public Task<BaseResponseModel> SetAsBookmarkedAsync(SetAsBookmarkedRequestModel model)
        {
            return requestProvider.PostAsync<BaseResponseModel>(SetAsBookmarkedEndpoint, model);
        }

        public async Task<CompanyInfoModel> GetCurrentCompanyInfo()
        {
            var res = await requestProvider.GetAsync<CompanyInfoModel>(GetCurrentCompanyInfoEndpoint);
            if (res.Projects == null || res.Projects.Count == 0)
            {
                await CreateProject(new ProjectModel() { Name = "Internal Project", Description = "Developing internal system", StartDate = DateTime.Now, AmountOfCandidates = 2 });
                await CreateProject(new ProjectModel() { Name = "Production Project", Description = "Developing production system", StartDate = DateTime.Now, AmountOfCandidates = 3 });

                res = await requestProvider.GetAsync<CompanyInfoModel>(GetCurrentCompanyInfoEndpoint);
            }
            return res;
        }

        public Task CreateProject(ProjectModel project)
        {
            return requestProvider.PostAsync<BaseResponseModel>(CreateProjectEndpoint, project);
        }

        //public Task<UserModel> GetUserById(string id)
        //{
        //    return Task.Run(() =>
        //    {
        //        var userModel = new UserModel
        //        {
        //            FirstName = "Nazar",
        //            LastName = "Terletskyy",
        //            CompanyPosition = "Software Engineer",
        //            Status = "Closed for the new positions",
        //            Location = "Lviv, Ukraine",
        //            Connections = 50,
        //            Intro = "Software Engineer with experience in developing mobile and web technologies. I was involved in development in different stages of the project life cycle, such as writing business logic, UI development and testing. I am open - minded and goal - oriented.I have active teamwork attitude, good interpersonal and communicational skills and quick learning ability.",
        //            LastUpdate = DateTimeOffset.Now,
        //            Education = new List<string> 
        //            { 
        //                "MSc Degree in Computer Science", 
        //                "Bachelor in Computer Science" 
        //            },
        //            Skills = new List<SkillModel>
        //            {
        //                new SkillModel { Title = "C#", IsTop = true},
        //                new SkillModel { Title = "Xamarin", IsTop = true},
        //                new SkillModel { Title = "Git", IsTop = false},
        //                new SkillModel { Title = "Software Architecture", IsTop = false},
        //                new SkillModel { Title = "Swift", IsTop = false}
        //            }
        //        };

        //        return userModel;
        //    });

        //}
    }
}

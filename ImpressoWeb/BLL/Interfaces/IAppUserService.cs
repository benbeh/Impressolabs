using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
    public interface IAppUserService
    {
        IEnumerable<AppUser> GetAll();

        AppUser Get(string id);

        CompanyProfileViewModel GetCompanyProfile(string userId);

        List<JobViewModel> GetCurrentCompanyJobsThatNotConnectedWithUser(string userId, string currentUserId);

        List<ProjectViewModel> GetCurrentCompanyProjectsThatNotSavedWithUser(string userId, string currentUserId);

        void UpdateUserProfile(string userId, EditUserProfileViewModel model, string oldUserPhoto);

        void SetEthereumAddress(string userId, string model);

        void ChangeCompanyImage(string userId, IFormFile image);

        void VerifyTestimonial(string userId, int testimonialId);

        PersonProfileViewModel GetPersonProfile(string id, string currentUserId);

        ConnectPersonCompanyViewModel GetConnectPersonCompanyViewModel(string userId);

        SavePersonForProjectViewModel GetSavePersonForProjectViewModel(string userId);

        PersonFilterValuesViewModel GetFilters();

        IEnumerable<AppUser> Filter(IEnumerable<AppUser> users, PersonFilterValuesViewModel filter);

        void AddSkill(AppUserSkillViewModel skill);

        void BlockUser(BlockedUserViewModel blockedUser);

        void UnblockUser(BlockedUserViewModel blockedUser);

        void BookmarkedUser(BookmarkedUserViewModel bookmarkedUser);

        void UnBookmarkedUser(BookmarkedUserViewModel bookmarkedUser);

        void BookmarkedJob(BookmarkedJobViewModel bookmarkedJob);

        void UnBookmarkedJob(BookmarkedJobViewModel bookmarkedJob);

        void InterestedEvent(InterestedEventViewModel interestedEvent);

        void UnInterestedEvent(InterestedEventViewModel interestedEvent);

        void ConnectUser(ConnectedUserViewModel connectedUser);

        bool CheckIfIsBookmarkedJob(string userId, int jobId);

        bool CheckIfIsConnectedPerson(string userId, string connectedAppUserId);

        bool CheckIfIsBookmarkedUser(string currentUserId, string userId);

        IEnumerable<TagViewModel> GetBookmakedUsersByUserId(string userId);

        void Dispose();

        object GetAPIFilters();

        IEnumerable<AppUser> APIFilter(PeopleFilterViewModel filter);

        List<string> GetLocations();
    }
}
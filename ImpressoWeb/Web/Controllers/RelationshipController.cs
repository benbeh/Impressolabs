using BLL.Interfaces;
using BLL.ViewModels;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class RelationshipController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;
        private readonly IProjectService _projectService;
        private readonly IJobService _jobService;

        public RelationshipController(UserManager<AppUser> userManager, IAppUserService appUserService, IProjectService projectService, IJobService jobService)
        {
            _userManager = userManager;
            _appUserService = appUserService;
            _projectService = projectService;
            _jobService = jobService;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ConnectPerson(string userId, int jobId, string returnUrl)
        {
            _jobService.AddUser(userId, jobId);
            return Redirect(returnUrl);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SavePersonForProject(string userId, int projectId, string returnUrl)
        {
            _projectService.AddUser(userId, projectId);
            return Redirect(returnUrl);
        }

        [HttpPost]
        public void BlockUser(string userId)
        {
            var currentUserId = _userManager.GetUserAsync(User).Result.Id;
            _appUserService.BlockUser(new BlockedUserViewModel { AppUserId = currentUserId, BlockedAppUserId = userId });
        }

        [HttpPost]
        public void UnblockUser(string userId)
        {
            var currentUserId = _userManager.GetUserAsync(User).Result.Id;
            _appUserService.UnblockUser(new BlockedUserViewModel { AppUserId = currentUserId, BlockedAppUserId = userId });
        }

        [HttpGet]
        public IActionResult GetCurrentCompanyJobsNotConnectedWithUser(string userId)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);
            return Json(_appUserService.GetCurrentCompanyJobsThatNotConnectedWithUser(userId, currentUser.Result.Id));
        }

        [HttpGet]
        public IActionResult GetCurrentCompanyProjectsNotSavedWithUser(string userId)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);
            return Json(_appUserService.GetCurrentCompanyProjectsThatNotSavedWithUser(userId, currentUser.Result.Id));
        }
    }
}
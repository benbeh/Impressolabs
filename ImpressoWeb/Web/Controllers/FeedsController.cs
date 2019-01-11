using BLL.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.ViewModels;
using Core.Enum;

namespace Web.Controllers
{
    [Authorize]
    public class FeedsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IAppUserService _appUserService;

        public FeedsController(UserManager<AppUser> userManager, INotificationService notificationService, IAppUserService appUserService)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _appUserService = appUserService;
        }

        public async Task<IActionResult> People()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var users = (await _userManager.GetUsersInRoleAsync(RoleEnum.User.ToString())).AsQueryable().
                Include(user => user.BlockedUsers).Include(user => user.CompanyAppUsers)
                .ThenInclude(companyAppUser => companyAppUser.Company).AsEnumerable();

            ViewBag.CurrentUser = _userManager.Users.Where(user => user.Id == currentUser.Id).Include(user => user.BlockedUsers).Include(user => user.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).First();
            ViewBag.ConnectPerson = _appUserService.GetConnectPersonCompanyViewModel(currentUser.Id);
            ViewBag.SavePersonForProject = _appUserService.GetSavePersonForProjectViewModel(currentUser.Id);

            return View(users);
        }

        public IActionResult Notifications()
        {
            var userId = _userManager.GetUserAsync(User).Result.Id;
            return View(_notificationService.GetAllByUserId(userId));
        }

        public void ChangeNewest()
        {
            var userId = _userManager.GetUserAsync(User).Result.Id;
            _notificationService.ChangeNewest(userId);
        }

        public IActionResult CloseNotification(int id)
        {
            _notificationService.Delete(id);
            return Redirect("/Feeds/Notifications");
        }

        public IActionResult Person(string userId)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);
            ViewBag.ConnectPerson = _appUserService.GetConnectPersonCompanyViewModel(currentUser.Result.Id);
            ViewBag.SavePersonForProject = _appUserService.GetSavePersonForProjectViewModel(currentUser.Result.Id);
            return View(_appUserService.GetPersonProfile(userId, User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        public void VerifyTestimonial(int testimonialId)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);
            _appUserService.VerifyTestimonial(currentUser.Result.Id, testimonialId);
        }

        public JsonResult GetCurrentUserPhoto()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);
            return Json(currentUser.Result.Photo);
        }
    }
}

using BLL.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BLL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using Core.Enum;
using Microsoft.EntityFrameworkCore;
using Web.Infrastructure;

namespace Web.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;

        public SearchController(UserManager<AppUser> userManager, IAppUserService appUserService)
        {
            _userManager = userManager;
            _appUserService = appUserService;
        }

        public async Task<IActionResult> Index(PersonFilterValuesViewModel filter)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.ConnectPerson = _appUserService.GetConnectPersonCompanyViewModel(currentUser.Id);
            ViewBag.Filter = filter;

            IEnumerable<AppUser> users = (await _userManager.GetUsersInRoleAsync(RoleEnum.User.ToString())).AsQueryable().Include(user => user.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).Include(user => user.AppUserSkills).ThenInclude(appUserSkill => appUserSkill.Skill).Include(user => user.AppUserCertificates).ThenInclude(appUserCertificate => appUserCertificate.Certificate);
            return View(_appUserService.Filter(users, filter));
        }
    }
}

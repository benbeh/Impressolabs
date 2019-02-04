using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using BLL.Helpers;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;
using System.Security.Claims;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICompanyService _companyService;

        public HomeController(RoleManager<IdentityRole> roleManager, ICompanyService companyService)
        {
            _roleManager = roleManager;
            _companyService = companyService;
        }

        [Authorize]
        public IActionResult Index() => View(GetData(nameof(Index)));

        [Authorize(Roles = "Users")]
        public IActionResult OtherAction() => View("Index", GetData(nameof(OtherAction)));

        private Dictionary<string, object> GetData(string actionName) =>
            new Dictionary<string, object>
            {
                ["Action"] = actionName,
                ["User"] = HttpContext.User.Identity.Name,
                ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
                ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
                ["In Users Role"] = string.Join(",", _roleManager.Roles.Where(role => HttpContext.User.IsInRole(role.Name)).Select(role => role.Name))
            };

        [Authorize]
        public JsonResult GetCurrentCompany()
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = _companyService.GetCurrentUserCompany(currentUserId);
            if (company == null)
                return null;
            return Json(new
            {
                Photo = company.CompanyLogoSource,
                Name = company.CompanyName,
                Industry = company.CompanyArea.GetDescription()
            });
        }
    }
}

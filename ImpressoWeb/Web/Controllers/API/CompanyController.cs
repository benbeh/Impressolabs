using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Integration;
using BLL.Interfaces;
using BLL.ViewModels;
using Core.Entities;
using Core.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Infrastructure;

namespace Web.Controllers.API
{
    [AuthorizeRoles(true)]
    [Route("api/[controller]/[action]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;



        public CompanyController(ICompanyService companyService, UserManager<AppUser> userManager, IAppUserService appUserService)
        {
            _userManager = userManager;
            _companyService = companyService;
            _appUserService = appUserService;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public JsonResult ListCompanies()
        {
            return Json(_companyService.GetAll());
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public JsonResult ListCompaniesByName(string name)
        {
            if(name == null)
            {
                return Json(_companyService.GetAll());
            }
            return Json(_companyService.GetAllByName(name));
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public JsonResult GetCompany(int Id)
        {
            return Json(_companyService.GetCompany(Id));
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public ActionResult GetFilters()
        {
            if (ModelState.IsValid)
            {
                return Ok(_companyService.GetFilters());
            }
            return null;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public ActionResult ListSortedCompanies(CompanyFilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                return Ok(_companyService.Filter(filter));
            }
            return null;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> GetCompanyProfile()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(_appUserService.GetCompanyProfile(user.Id));

        }
    }
}

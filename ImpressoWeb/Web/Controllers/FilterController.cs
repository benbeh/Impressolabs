using BLL.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BLL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Web.Infrastructure;

namespace Web.Controllers
{
    public class FilterController : Controller
    {
        private readonly IAppUserService _appUserService;

        public FilterController(UserManager<AppUser> userManager, IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public ActionResult GetFilters()
        {
            if (ModelState.IsValid)
            {
                return Ok(_appUserService.GetFilters());
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetPeopleLocations()
        {
            return Json(_appUserService.GetLocations());
        }
    }
}

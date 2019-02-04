using BLL.Integration;
using BLL.Interfaces;
using Core.Entities;
using Core.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Infrastructure;

namespace Web.Controllers.API
{
    [AuthorizeRoles(true)]
    [Route("api/[controller]/[action]")]
    public class TagController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJobService _jobService;
        private readonly IEventService _eventService;
        private readonly IAppUserService _appUserService;

        public TagController(IJobService jobService, IAppUserService appUserService, IEventService eventService, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _jobService = jobService;
            _eventService = eventService;
            _appUserService = appUserService;
        }


        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> ListTags()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            var jobs = _jobService.GetBookmakedJobsByUserId(user.Id);
            var people = _appUserService.GetBookmakedUsersByUserId(user.Id);

            var result = jobs.Concat(people);

            result = result.OrderBy(tag => tag.BookmarkedData).ToList();
            return Json(result);
        }
    }
}

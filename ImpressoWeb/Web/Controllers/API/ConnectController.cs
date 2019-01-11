using BLL.AutoMapper;
using BLL.Integration;
using BLL.Interfaces;
using BLL.ViewModels.API;
using Core.Entities;
using Core.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Infrastructure;

namespace Web.Controllers.API
{
    [AuthorizeRoles(true)]
    [Route("api/[controller]/[action]")]
    public class ConnectController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEventService _eventService;
        private readonly IAppUserService _appUserService;

        public ConnectController(UserManager<AppUser> userManager, IEventService eventService, IAppUserService appUserService)
        {
            _userManager = userManager;
            _eventService = eventService;
            _appUserService = appUserService;
        }

        [AuthorizeRoles(true, RoleEnum.User, RoleEnum.HiringManager, RoleEnum.Admin)]
        [HttpGet]
        public async Task<JsonResult> ListEventsAndPeople()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            var users = _userManager.Users.Include(u => u.BookmarkedByUsers).Include(u => u.ConnectedByUsers).ToList();
            var people = Mapping.Map<IEnumerable<AppUser>, IEnumerable<PeopleConnectViewModel>>(users, opts => opts.Items["CurrentUserId"] = user.Id);
            return Json(new ConnectViewModel { Events = _eventService.GetListEventsByUserId(user.Id), People = people});
        }

        [AuthorizeRoles(true, RoleEnum.User, RoleEnum.HiringManager, RoleEnum.Admin)]
        [HttpPost]
        public async Task<ResponseMessage> ConnectPerson([FromBody]string userId)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            var isConnected = _appUserService.CheckIfIsConnectedPerson(user.Id, userId);
            if(isConnected)
                return new ResponseMessage { Message = "You have already been connected with this person", IsSuccess = false };
            else
            {
                _appUserService.ConnectUser(new ConnectedUserViewModel { AppUserId = user.Id, ConnectedAppUserId = userId });
                _appUserService.ConnectUser(new ConnectedUserViewModel { AppUserId = userId, ConnectedAppUserId = user.Id });

            }

            return new ResponseMessage { IsSuccess = true };
        }
    }
}

using BLL.Integration;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.API;
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
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;

        public EventController(IEventService eventService, UserManager<AppUser> userManager, IAppUserService appUserService)
        {
            _eventService = eventService;
            _userManager = userManager;
            _appUserService = appUserService;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> ListEvents()
        {
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(_eventService.GetListEventsByUserId(user.Id));
        }

        //[AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        //[HttpPost]
        //public async Task<ResponseMessage> CreateEvent([FromBody]CreateEventViewModel model)
        //{
        //    //get current user from jwt token
        //    var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
        //    var user = await _userManager.FindByNameAsync(userName);

        //    if (ModelState.IsValid)
        //    {
        //        _eventService.CreateEvent(user.Id, model);
        //        return new ResponseMessage { IsSuccess = true };
        //    }
        //    else
        //    {
        //        return new ResponseMessage { IsSuccess = false, Message = "Model is not valid" };
        //    }
        //}

        //[AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        //[HttpPost]
        //public ResponseMessage UpdateEvent([FromBody]EventViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _eventService.Update(model);
        //        return new ResponseMessage { IsSuccess = true };
        //    }
        //    else
        //    {
        //        return new ResponseMessage { IsSuccess = false, Message = "Model is not valid" };
        //    }
        //}

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<ResponseMessage> SetAsInterested([FromBody]SetAsBookmarkedViewModel model)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            if (model.IsInterested == true)
            {
                _appUserService.InterestedEvent(new InterestedEventViewModel { AppUserId = user.Id, EventId = model.EventId });
                return new ResponseMessage { IsSuccess = true, Message = "Event is interested" };
            }
            else
            {
                _appUserService.UnInterestedEvent(new InterestedEventViewModel { AppUserId = user.Id, EventId = model.EventId });
                return new ResponseMessage { IsSuccess = true, Message = "Event is uninterested" };
            }
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> ListEventsForCurrentUser()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            var events = _eventService.GetBookmakedEventsByUserId(user.Id);
            return Json(events);
        }
    }
}

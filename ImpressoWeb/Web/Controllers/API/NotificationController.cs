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
    public class NotificationController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;

        public NotificationController(UserManager<AppUser> userManager, INotificationService notificationService)
        {
            _userManager = userManager;
            _notificationService = notificationService;
        }

        [AuthorizeRoles(true, RoleEnum.User, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> ListNotifications()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);
            

            return Json(_notificationService.GetAllByUserId(user.Id));
        }

        [AuthorizeRoles(true,RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public ResponseMessage CreateNotification([FromBody]NotificationViewModel notification)
        {
            if(ModelState.IsValid)
            {
                _notificationService.Add(notification);
                return new ResponseMessage { IsSuccess = true };
            }
            else
            {
                return new ResponseMessage { IsSuccess = false, Message = "Model is not valid" };
            }
            
        }
    }
}

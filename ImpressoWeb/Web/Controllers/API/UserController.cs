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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.AutoMapper;
using BLL.ViewModels;
using Web.Infrastructure;

namespace Web.Controllers.API
{
    [AuthorizeRoles(true)]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;

        public UserController(UserManager<AppUser> userManager, IAppUserService appUserService)
        {
            _userManager = userManager;
            _appUserService = appUserService;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> GetCurrentUserNameWithPhoto()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(new
            {
                user.UserName,
                user.Photo,
                FullName = user.FirstName + " " + user.LastName
            });
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<ResponseMessage> EditUserProfile([FromBody]EditUserProfileViewModel model)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            _appUserService.UpdateUserProfile(user.Id, model, user.Photo);
            return new ResponseMessage { IsSuccess = true };
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<ResponseMessage> SetEthereumAddress([FromBody]string model)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            _appUserService.SetEthereumAddress(user.Id, model);
            return new ResponseMessage { IsSuccess = true };
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> GetUserProfileInfoById(string userId)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var currentUser = await _userManager.FindByNameAsync(userName);

            var result = _appUserService.GetPersonProfile(userId, currentUser.Id);

            return Json(result);
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<ResponseMessage> SetAsBookmarked([FromBody]BookmarkUserViewModel model)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            var Bookmarked = _appUserService.CheckIfIsBookmarkedUser(user.Id, model.Id);

            if (model.IsBookmarked == true)
            {
                if (Bookmarked)
                    return new ResponseMessage { Message = "User has been already bookmarked", IsSuccess = false };

                _appUserService.BookmarkedUser(new BookmarkedUserViewModel { AppUserId = user.Id, BookmarkedAppUserId = model.Id });
                return new ResponseMessage { IsSuccess = true, Message = "User is bookmarked" };
            }
            else
            {
                if (!Bookmarked)
                    return new ResponseMessage { Message = "User is not bookmarked", IsSuccess = false };

                _appUserService.UnBookmarkedUser(new BookmarkedUserViewModel { AppUserId = user.Id, BookmarkedAppUserId = model.Id });
                return new ResponseMessage { IsSuccess = true, Message = "User is un bookmarked" };
            }

        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public ActionResult GetFilters()
        {
            if (ModelState.IsValid)
            {
                return Ok(_appUserService.GetAPIFilters());
            }
            return null;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public ActionResult ListSortedUsers(PeopleFilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                var users = _appUserService.APIFilter(filter);
                return Ok(Mapping.Map<IEnumerable<AppUser>, IEnumerable<PersonProfileViewModel>>(users));
            }
            return null;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet("claims")]
        public object GetInformationFromJWT()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }
    }
}

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
    public class TokenController : Controller
    {
        private readonly ITokenLogService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public TokenController(UserManager<AppUser> userManager, ITokenLogService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> ListTokens()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(_tokenService.GetAll().Where(record => record.Receiver == user.Id));
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> GetTotalTokensCount()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(_tokenService.GetTotalTokensCount(user.Id));
        }

    }
}

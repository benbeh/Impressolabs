using BLL.Integration;
using BLL.Interfaces;
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
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly UserManager<AppUser> _userManager;

        public ProjectController(UserManager<AppUser> userManager, IProjectService projectService)
        {
            _projectService = projectService;
            _userManager = userManager;
        }

        [AuthorizeRoles(true, RoleEnum.HiringManager, RoleEnum.User, RoleEnum.Admin)]
        [HttpPost]
        public async Task<ResponseMessage> CreateProject([FromBody] ProjectViewModel project)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            if (ModelState.IsValid)
            {
                _projectService.CreateProject(user.Id, project);
                return new ResponseMessage { IsSuccess = true };
            }
            else
            {
                return new ResponseMessage { IsSuccess = false, Message = "Model is not valid" };
            }
        }

    }
}

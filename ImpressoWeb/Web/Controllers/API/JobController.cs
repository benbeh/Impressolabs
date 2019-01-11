using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.ViewModels;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Web.ApiAuthentication.Helpers;
using Web.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BLL.Integration;
using BLL.ViewModels.API;
using Core.Enum;

namespace Web.Controllers.API
{
    [AuthorizeRoles(true)]
    [Route("api/[controller]/[action]")]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;

        public JobController(IJobService jobService, UserManager<AppUser> userManager, IAppUserService appUserService)
        {
            _jobService = jobService;
            _userManager = userManager;
            _appUserService = appUserService;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public JsonResult ListJobsByName(string name)
        {
            if(name == null)
            {
                return Json(_jobService.GetAll());
            }
            return Json(_jobService.GetAllByName(name));
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> ListJobs()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(_jobService.GetListJobs(user.Id));
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> ListMatchedSkillsByJob([FromBody]int Id)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(_jobService.GetTopSkillsMatch(user.Id, Id));
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public ResponseMessage CreateJob([FromBody]CreateJobViewModel job)
        {
            if (ModelState.IsValid)
            {
                _jobService.CreateJob(job);
                return new ResponseMessage { IsSuccess = true };
            }
            else
            {
                return new ResponseMessage { IsSuccess = false, Message = "Model is not valid" };
            }
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> GetJobInfo(int Id)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(_jobService.GetJobInfo(user.Id, Id));
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public ActionResult GetFilters()
        {
            if (ModelState.IsValid)
            {
                return Ok(_jobService.GetFilters());
            }
            return null;
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public ActionResult ListSortedJobs(JobFilterViewModel filter)
        {
            if (ModelState.IsValid)
            {
                return Ok(_jobService.Filter(filter));
            }
            return null;
        }
    

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<ResponseMessage> SetAsBookmarked([FromBody]BookmarkViewModel model)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            var Bookmarked = _appUserService.CheckIfIsBookmarkedJob(user.Id, model.Id);

            if (model.IsBookmarked == true)
            {
                if (Bookmarked)
                    return new ResponseMessage { Message = "Job has been already bookmarked", IsSuccess = false };

                _appUserService.BookmarkedJob(new BookmarkedJobViewModel { AppUserId = user.Id, JobId = model.Id });
                return new ResponseMessage { IsSuccess = true, Message = "Job is bookmarked" };
            }
            else
            {
                if (!Bookmarked)
                    return new ResponseMessage { Message = "Job is not bookmarked", IsSuccess = false };

                _appUserService.UnBookmarkedJob(new BookmarkedJobViewModel { AppUserId = user.Id, JobId = model.Id });
                return new ResponseMessage { IsSuccess = true, Message = "Job is un bookmarked" };
            }

        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<ResponseMessage> ApplyForJob([FromBody]IdViewModel model)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);
            
            var result =_jobService.ApplyForProjectByUserId(new AppUserJobViewModel { AppUserId = user.Id, JobId = model.Id });

            if (result)
            {
                return new ResponseMessage { IsSuccess = true };
            }
            else
            {
                return new ResponseMessage { IsSuccess = false, Message = "You have already applied for a job" };
            }
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<JsonResult> AppliedJobsOfCurrentUser()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            var result = _jobService.ListAppliedJobsOfCurrentUser(user.Id);

            return Json(result);
        }


    }
}

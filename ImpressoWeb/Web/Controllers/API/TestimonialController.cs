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
    public class TestimonialController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITestimonialService _testimonialService;
        private readonly IAppUserService _appUserService;

        public TestimonialController(UserManager<AppUser> userManager, ITestimonialService testimonialService, IAppUserService appUserService)
        {
            _userManager = userManager;
            _testimonialService = testimonialService;
            _appUserService = appUserService;
        }


        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<Object> CreateTestimonial([FromBody]string Content)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            if (ModelState.IsValid)
            {
                var testimonial = _testimonialService.CreateTestimonial(user.Id, Content);
                return new { IsSuccess = true, TestimonialId = testimonial.Id };
            }
            else
            {
                return new ResponseMessage { IsSuccess = false, Message = "Model is not valid" };
            }
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<ResponseMessage> VerifyTestimonial([FromBody]IdViewModel model)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            _appUserService.VerifyTestimonial(user.Id, model.Id); 
            return new ResponseMessage { IsSuccess = true };
        }

        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpPost]
        public async Task<ResponseMessage> EditTestimonial([FromBody]EditTestimonialViewModel model)
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            if (ModelState.IsValid)
            {
                _testimonialService.EditTestimonial(model.testimonialId, model.Content);
                return new ResponseMessage { IsSuccess = true };
            }
            else
            {
                return new ResponseMessage { IsSuccess = false, Message = "Model is not valid" };
            }
        }



        [AuthorizeRoles(true, RoleEnum.Admin, RoleEnum.HiringManager, RoleEnum.User)]
        [HttpGet]
        public async Task<JsonResult> GetInformationNeededForTestimonial()
        {
            //get current user from jwt token
            var userName = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(userName);

            return Json(new
            {
                NameOfUser = user.UserName,
                Position = user.Profession,
                Date = DateTime.Now
            });
        }
    }
}

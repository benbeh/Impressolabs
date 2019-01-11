using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using BLL.AutoMapper;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;
using Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Infrastructure;

namespace Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISkillService _skillService;
        private readonly IJobService _jobService;
        private readonly IProjectService _projectService;
        private readonly IAppUserService _appUserService;
        private readonly ICompanyService _companyService;
        private readonly ICertificateService _certificateService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProfileController(UserManager<AppUser> userManager, ISkillService skillService, IJobService jobService, IProjectService projectService, IAppUserService appUserService, ICompanyService companyService, ICertificateService certificateService, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _skillService = skillService;
            _jobService = jobService;
            _projectService = projectService;
            _appUserService = appUserService;
            _companyService = companyService;
            _certificateService = certificateService;
            _hostingEnvironment = environment;
        }

        public IActionResult Company()
        {
            return View(_appUserService.GetCompanyProfile(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        public IActionResult ProjectDetails(int projectId)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);
            ViewBag.ConnectPerson = _appUserService.GetConnectPersonCompanyViewModel(currentUser.Result.Id);
            return View(_projectService.GetProjectWithUsersAndJobs(projectId));
        }

        public IActionResult DeleteProject(int id)
        {
            _projectService.Delete(id);
            return RedirectToAction("Company");
        }

        [HttpPost]
        public JsonResult CreateProject(string title, string description)
        {
            if (title == null)
                throw new Exception("Name can't be null. Create project");
            var currentUser = _userManager.Users.Include(user => user.CompanyAppUsers).First(user => user.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentCompanyId = currentUser.CompanyAppUsers.First().CompanyId;
            return Json(_projectService.Add(new ProjectViewModel() {Name = title, Description = description, CompanyId = currentCompanyId}).Id);
        }

        public IActionResult EditProject(int projectId)
        {
            return View(_projectService.Get(projectId));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditProject(ProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                _projectService.Update(project);
                return RedirectToAction("ProjectDetails", new { projectId = project.Id });
            }
            return View(project);
        }

        [HttpPost]
        public IActionResult CreateJob(string name, int projectId)
        {
            if (name == null)
                throw new Exception("Name can't be null. Create job");
            var job = _jobService.Add(new JobViewModel() { Title = name, ProjectId = projectId});
            return RedirectToAction("EditJob", new {jobId = job.Id});
        }

        public IActionResult EditJob(int jobId)
        {
            ViewBag.Certificates = _certificateService.GetAllAsNoTracking();
            ViewBag.Skills = _skillService.GetAllAsNoTracking();
            return View(_jobService.GetJobDetailsViewModel(jobId));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditJob(JobDetailsViewModel job)
        {
            if (ModelState.IsValid)
            {
                // save image to folder
                string filePath = "";
                if (job.PhotoFile != null && job.PhotoFile.Length > 0)
                {
                    filePath = "/images/Jobs/" + job.Id + ".png";

                    using (var stream = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                    {
                        job.PhotoFile.CopyTo(stream);
                    }
                }
                // update
                job.Photo = filePath;
                _jobService.Update(job);
                return RedirectToAction("JobDetails", new { jobId = job.Id });
            }
            ViewBag.Certificates = _certificateService.GetAllAsNoTracking();
            ViewBag.Skills = _skillService.GetAllAsNoTracking();
            return View(job);
        }

        public void ChangeProjectName(int id, string name)
        {
            if (!ModelState.IsValid)
                return;
            _projectService.ChangeProjectName(id, name);
        }

        [HttpGet]
        public IActionResult JobDetails(PersonFilterValuesViewModel filter)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User);
            ViewBag.ConnectPerson = _appUserService.GetConnectPersonCompanyViewModel(currentUser.Result.Id);
            ViewBag.Filter = filter;

            var job = _jobService.GetJobDetailsViewModel(filter.JobId);
            IEnumerable<AppUser> users = _userManager.Users.Include(user => user.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).Include(user => user.AppUserSkills).ThenInclude(appUserSkill => appUserSkill.Skill).Include(user => user.AppUserCertificates).ThenInclude(appUserCertificate => appUserCertificate.Certificate)
                .Where(user => job.AppUsers.Any(appUser => appUser.Id == user.Id));
            users = _appUserService.Filter(users, filter);
            job.AppUsers = Mapping.Map<IEnumerable<AppUser>, List<AppUserViewModel>>(users);

            return View(job);
        }

        [HttpPost]
        public async Task ChangeCompanyImage(IFormFile file)
        {
            _appUserService.ChangeCompanyImage(User.FindFirstValue(ClaimTypes.NameIdentifier), file);
        }

        [HttpGet]
        public IActionResult Applicants(PersonFilterValuesViewModel filter)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentCompanyId = _companyService.GetCurrentUserCompany(currentUserId).Id;

            ViewBag.ConnectPerson = _appUserService.GetConnectPersonCompanyViewModel(currentUserId);
            ViewBag.Filter = filter;

            var applicants = _companyService.GetAllApplicants(currentCompanyId);

            // filter users
            var users = _appUserService.Filter(applicants.Select(applicant => applicant.AppUser), filter);

            // sort result
            applicants = applicants.Where(applicant => users.Any(user => user.Id == applicant.Id));

            return View(new ApplicantsListsViewModel(applicants));
        }

        public IActionResult EditCompanyProfile()
        {
            var company = _companyService.GetCurrentUserCompany(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Industries = Enum.GetValues(typeof(IndustryEnum));
            return View(company);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditCompanyProfile(CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                // save image to folder
                string filePath = "";
                if (company.PhotoFile != null && company.PhotoFile.Length > 0)
                {
                    filePath = "/images/Companies/" + company.Id + ".png";

                    using (var stream = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                    {
                        await company.PhotoFile.CopyToAsync(stream);
                    }
                }
                // update
                company.CompanyLogoSource = filePath;
                _companyService.Update(company);
                return RedirectToAction("Company");
            }
            return View(company);
        }

        [HttpGet]
        public JsonResult GetCompaniesLocations()
        {
            return Json(_companyService.GetLocations());
        }

        [HttpGet]
        public JsonResult GetJobsLocations()
        {
            return Json(_jobService.GetLocations());
        }
    }
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.Account;
using BLL.ViewModels.API;
using Core.Entities;
using Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Infrastructure;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ICompanyService _companyService;
        private readonly ITokenLogService _tokenLogService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AccountController(UserManager<AppUser> userManager, ITokenLogService tokenLogService, SignInManager<AppUser> signInManager, IEmailSender emailSender, IConfiguration configuration, ICompanyService companyService, RoleManager<IdentityRole> roleManager, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _tokenLogService = tokenLogService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _companyService = companyService;
            _hostingEnvironment = environment;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public IActionResult Slider(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthorizationViewModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    if (!_userManager.IsEmailConfirmedAsync(user).Result)
                    {
                        ModelState.AddModelError("", "Email not confirmed!");
                        return View(details);
                    }

                    var role = await _userManager.GetRolesAsync(user);
                    if (role.All(roleName => roleName != RoleEnum.HiringManager.ToString()))
                    {
                        ModelState.AddModelError("", "Only Hiring Manager can log in");
                        return View(details);
                    }

                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect("/Feeds/People");
                    }
                }
                ModelState.AddModelError(nameof(AuthorizationViewModel.Email), "Invalid user or password");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult RegistrationFirstStep(string returnUrl, string email = null, bool isCompanyChecked = false)
        {
            ViewBag.returnUrl = returnUrl;
            if (email != null)
                return View(new RegistrationFirstStepViewModel() {Email = email, IsCompanyChecked = isCompanyChecked});
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrationFirstStep(RegistrationFirstStepViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsCompanyChecked)
                {
                    model.Role = RoleEnum.HiringManager;
                    return View("RegistrationSecondStepCompany", new RegistrationSecondStepCompanyViewModel() {FirstStepModel = model});
                }
                else
                {
                    model.Role = RoleEnum.User;
                    return StatusCode(404);
                }
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationSecondStepCompany(RegistrationSecondStepCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.FirstStepModel.Email.Split("@").First(),
                    Email = model.FirstStepModel.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.FirstStepModel.Password);
                if (result.Succeeded)
                {
                    _tokenLogService.AddTokensForSingingIn(user.Id);

                    // add to role
                    string role = model.FirstStepModel.Role.ToString();
                    if (await _roleManager.FindByNameAsync(role) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await _userManager.AddToRoleAsync(user, role);

                    // check if company exists
                    if (!_companyService.GetAllByName(model.CompanyName).Any())
                    {
                        // company doesn't exist

                        var companyViewModel = new CompanyViewModel()
                        {
                            CompanyName = model.CompanyName,
                        };

                        _companyService.Add(companyViewModel);

                        _companyService.DetachAllEntities();

                        var resultCompany = _companyService.GetAllAsNoTracking()
                            .First(tempCompany => tempCompany.CompanyName == model.CompanyName);

                        // save image to folder
                        string filePath = "";
                        if (model.Image != null && model.Image.Length > 0)
                        {
                            filePath = "/images/Companies/" + resultCompany.Id + ".png";

                            using (var stream = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                            {
                                await model.Image.CopyToAsync(stream);
                            }
                        }

                        resultCompany.CompanyLogoSource = filePath;
                        _companyService.Update(resultCompany);
                    }

                    // add companyAppUser
                    var company = _companyService.GetAllByName(model.CompanyName).First();
                    _companyService.AddUser(new CompanyAppUserViewModel() {AppUserId = user.Id, CompanyId = company.Id, IsWorkingNow = true});

                    //send confirmation email
                    string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                    string confirmationLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        code = confirmationToken
                    }, Request.HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>link</a>", _configuration);

                    return View("CheckEmail");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationSecondStepUser(RegistrationSecondStepUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.FirstName + " " + model.LastName,
                    Email = model.FirstStepModel.Email,
                    Photo = String.IsNullOrEmpty(model.Photo) ? null : model.Photo
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.FirstStepModel.Password);
                if (result.Succeeded)
                {
                    // add to role
                    string role = model.FirstStepModel.Role.ToString();
                    if (await _roleManager.FindByNameAsync(role) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await _userManager.AddToRoleAsync(user, role);

                    //send confirmation email
                    string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                    string confirmationLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        code = confirmationToken
                    }, Request.HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>link</a>", _configuration);

                    return View("CheckEmail");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>", _configuration);
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
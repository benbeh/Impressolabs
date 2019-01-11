using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.ApiAuthentication.Helpers;
using Microsoft.Extensions.Configuration;
using BLL.Interfaces;
using System.Text.Encodings.Web;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Web.ApiAuthentication.Models;
using Web.ApiAuthentication.Auth;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using BLL.Integration;
using BLL.ViewModels;
using BLL.ViewModels.API;
using BLL.ViewModels.Account;
using Core;
using System.Linq;
using Core.Enum;
using System.IO;

namespace Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IJwtFactory _jwtFactory;

        private readonly FacebookAuthSettings _fbAuthSettings;
        private static readonly HttpClient Client = new HttpClient();
        private readonly JwtIssuerOptions _jwtOptions;

        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        private readonly IHostingEnvironment _environment;

        private readonly ImpressoDbContext _appDbContext;
        private readonly ICompanyService _companyService;
        private readonly ITokenLogService _tokenLogService;

        private readonly IHostingEnvironment _hostingEnvironment;


        public AccountController(UserManager<AppUser> userManager,ITokenLogService tokenLogService, ICompanyService companyService, IJwtFactory jwtFactory, SignInManager<AppUser> signInManager, IEmailSender emailSender, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IOptions<JwtIssuerOptions> jwtOptions, IOptions<FacebookAuthSettings> fbAuthSettingsAccessor, IHostingEnvironment environment, ImpressoDbContext appDbContext)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _roleManager = roleManager;
            _fbAuthSettings = fbAuthSettingsAccessor.Value;
            _jwtOptions = jwtOptions.Value;
            _hostingEnvironment = environment;
            _appDbContext = appDbContext;
            _companyService = companyService;
            _tokenLogService = tokenLogService;
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<ResponseMessage> Register([FromBody]RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                string role = model.Role.ToString();
                var userName = model.Email.Split('@').FirstOrDefault();
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                AppUser user = new AppUser
                {
                    Email = model.Email,
                    UserName = userName
                };

                if(model.Role == RoleEnum.User)
                {
                    // save photo
                    string filePath = null;
                    if (!String.IsNullOrEmpty(model.Base64Img))
                    {
                        filePath = "/images/Users/" + user.Id + "_0.png";
                        var bytes = Convert.FromBase64String(model.Base64Img);
                        using (var imageFile = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                    }

                    user.Photo = filePath;
                    user.LastName = model.LastName;
                    user.FirstName = model.FirstName;
                }
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if(model.Role == RoleEnum.HiringManager)
                {
                    // check if company exists
                    if (!_companyService.GetAllByName(model.CompanyName).Any())
                    {
                        // company doesn't exist
                        _companyService.Add(new CompanyViewModel() { CompanyName = model.CompanyName });

                        _companyService.DetachAllEntities();

                        var resultCompany = _companyService.GetAllAsNoTracking()
                            .First(tempCompany => tempCompany.CompanyName == model.CompanyName);

                        // save image to folder
                        string filePath = null;
                        if (!String.IsNullOrEmpty(model.Base64Img))
                        {
                            filePath = "/images/Users/" + user.Id + "_0.png";
                            var bytes = Convert.FromBase64String(model.Base64Img);
                            using (var imageFile = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                            }
                        }

                        resultCompany.CompanyLogoSource = filePath;
                        _companyService.Update(resultCompany);
                    }

                    // add companyAppUser
                    var company = _companyService.GetAllByName(model.CompanyName).First();
                    _companyService.AddUser(new CompanyAppUserViewModel() { AppUserId = user.Id, CompanyId = company.Id });
                }

                if (result.Succeeded)
                {
                    _tokenLogService.AddTokensForSingingIn(user.Id);

                    //send confirmation email
                    string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    string confirmationLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        code = confirmationToken
                    }, Request.HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>link</a>", _configuration);

                    await _userManager.AddToRoleAsync(user, role);
                    return new ResponseMessage() { IsSuccess = true };
                }
                else
                {
                    string errors = "";
                    foreach (IdentityError error in result.Errors)
                    {
                        errors += error.Code + ";";
                    }
                    return new ResponseMessage() { IsSuccess = false, Message = errors };
                }
            }
            return null;
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<object> Login([FromBody]AuthorizationViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await _userManager.FindByEmailAsync(credentials.Email);

            //check if user exists
            if (user == null)
            {
                return new ResponseMessage() { IsSuccess = false, Message = "User not found" };
            }

            //check if user has confirmed his email
            if (!_userManager.IsEmailConfirmedAsync(user).Result)
            {
                return new ResponseMessage() { IsSuccess = false, Message = "Email not confirmed" };
            }

            var identity = await GetClaimsIdentity(user.UserName, credentials.Password);

            if (identity == null)
            {
                return new ResponseMessage() { IsSuccess = false, Message = "Invalid username or password" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id),
              _jwtFactory, user.UserName, user.FirstName, user.LastName, roles, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            var jwtObj = JsonConvert.DeserializeObject<JwtViewModel>(jwt);

            return new JwtLoginResponseMessage { IsSuccess = true, Jwt = jwtObj };
        }

        [HttpPost]
        [ActionName("LoginFacebook")]
        public async Task<object> Facebook([FromBody]FacebookAuthViewModel model)
        {
            string role = "User";
            var url = $"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials";
            // 1.generate an app access token
            var appAccessTokenResponse = await Client.GetStringAsync(url);
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                return new ResponseMessage() { IsSuccess = false, Message = "Invalid facebook token" };
            }

            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            // 4. ready to create the local user account (if necessary) and jwt
            var userName = userInfo.Email.Split("@").FirstOrDefault();
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                var appUser = new AppUser
                {
                    UserName = userName
                };
                if (!string.IsNullOrEmpty(userInfo.Email))
                {
                    appUser.Email = userInfo.Email;
                }
                if (!string.IsNullOrEmpty(userInfo.FirstName))
                {
                    appUser.FirstName = userInfo.FirstName;
                }
                if (!string.IsNullOrEmpty(userInfo.LastName))
                {
                    appUser.LastName = userInfo.LastName;
                }

                //if (!string.IsNullOrEmpty(userInfo.Picture?.Data?.Url))
                //{
                //appUser.Photo = userInfo.Picture?.Data?.Url;
                //}
                appUser.Photo = string.Format("http://graph.facebook.com/{0}/picture?type=large", userInfo.Id);

                var result = await _userManager.CreateAsync(appUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

                if (!result.Succeeded)
                    return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

                var newUser = await _userManager.FindByNameAsync(userName);
                await _userManager.AddToRoleAsync(newUser, role);
            }

            // generate the jwt for the local user...
            var localUser = await _userManager.FindByNameAsync(userName);

            if (localUser == null)
            {
                return new ResponseMessage() { IsSuccess = false, Message = "Failed to create local user account" };
            }


            var roles = await _userManager.GetRolesAsync(localUser);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, localUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, localUser.UserName),
            };
            foreach (var tempRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, tempRole));
            }

            var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id),
                _jwtFactory, localUser.UserName, localUser.FirstName, localUser.LastName, roles, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            var jwtObj = JsonConvert.DeserializeObject<JwtViewModel>(jwt);

            return new JwtLoginResponseMessage { IsSuccess = true, Jwt = jwtObj };
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}

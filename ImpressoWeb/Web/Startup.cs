using BLL.Interfaces;
using BLL.Services;
using Core;
using Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;
using DAL.Interfaces;
using Microsoft.Extensions.Logging;
using Web.ApiAuthentication.Auth;
using Web.ApiAuthentication.Helpers;
using Web.ApiAuthentication.Models;
using Web.Infrastructure;
using Web.Logger;
using Swashbuckle.AspNetCore.Examples;
using Web.Helpers;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore.Design;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAppUserJobService, AppUserJobService>();
            services.AddTransient<ISkillService, SkillService>();
            services.AddTransient<ICertificateService, CertificateService>();
            services.AddTransient<IAppUserService, AppUserService>();
            services.AddTransient<IJobService, JobService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ITestimonialService, TestimonialService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<ITokenLogService, TokenLogService>();

            // connect to db
            services.AddDbContext<ImpressoDbContext>(options =>
                options.UseMySql(Configuration["Data:ConnectionString"]));

            // set up IdentityRole 
            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ImpressoDbContext>()
                .AddDefaultTokenProviders();

            //add swagger
            services.AddSwaggerDocumentation();

            // Register the ConfigurationBuilder instance of FacebookAuthSettings
            services.Configure<FacebookAuthSettings>(Configuration.GetSection(nameof(FacebookAuthSettings)));

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions["SecretKey"]));
            
            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication()
                .AddCookie(config => { config.SlidingExpiration = true; })
                .AddJwtBearer(configureOptions =>
                {
                    configureOptions.RequireHttpsMetadata = false;
                    configureOptions.SaveToken = true;
                    configureOptions.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            loggerFactory.AddDataBase(Configuration);

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSwaggerDocumentation();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc(routing =>
            {
                routing.MapRoute("api", "api/{controller}/{action}/{id?}");
            });

            app.UseStatusCodePagesWithRedirects("~/Errors/AppError{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Feeds}/{action=People}");
            });

            ImpressoDbContext.CreateAppUserAccount(app.ApplicationServices, Configuration, "Data:AdminUser").Wait();
            ImpressoDbContext.CreateAppUserAccount(app.ApplicationServices, Configuration, "Data:User").Wait();
            ImpressoDbContext.CreateAppUserAccount(app.ApplicationServices, Configuration, "Data:HiringManager").Wait();
        }
    }
}

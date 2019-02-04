using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AutoMapper;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.API;
using DAL.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Enum;
using Microsoft.AspNetCore.Identity;
using Remotion.Linq.Clauses;

namespace BLL.Services
{
    public class CompanyService : Service<Company, CompanyViewModel>, ICompanyService
    {
        private readonly UserManager<AppUser> _userManager;

        public CompanyService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager) : base(unitOfWork, unitOfWork.Companies)
        {
            _userManager = userManager;
        }


        public IEnumerable<CompanyViewModel> GetAllByName(string name)
        {
            return Mapping.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(Database.Companies.GetAll()
                .Where(company => company.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public void AddUser(CompanyAppUserViewModel companyAppUser)
        {
            Database.CompanyAppUsers.Add(Mapping.Map<CompanyAppUserViewModel, CompanyAppUser>(companyAppUser));
            Database.Save();
        }

        public CompanyViewModel GetCompany(int companyId)
        {
            var result = Database.Companies.GetAll().Include(company => company.Projects).ThenInclude(project => project.Jobs).ThenInclude(job => job.AppUserJobs).FirstOrDefault(company => company.Id == companyId);
            return Mapping.Map<Company, CompanyViewModel>(result);
        }

        public CompanyViewModel GetCurrentUserCompany(string userId)
        {
            var companyAppUser = Database.CompanyAppUsers.GetAll().Include(companyAppUsers => companyAppUsers.Company).ThenInclude(company => company.CompanyAppUsers)
                .First(companyAppUsers => companyAppUsers.AppUserId == userId);
            if (companyAppUser == null)
                return null;
            return Mapping.Map<Company, CompanyViewModel>(companyAppUser.Company);
        }

        public IEnumerable<ApplicantViewModel> GetAllApplicants(int companyId)
        {
            // select project in particular company
            var projects = Database.Projects.GetAll().Include(project => project.Jobs).ThenInclude(job => job.AppUserJobs)
                .Where(project => project.CompanyId == companyId);
            // select people from all jobs in projects
            var applicants = projects.SelectMany(project => project.Jobs).SelectMany(job => job.AppUserJobs).Include(appUserJob => appUserJob.AppUser)
                // data needed for filter
                .ThenInclude(user => user.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).Include(appUserJob => appUserJob.AppUser).ThenInclude(user => user.AppUserSkills).ThenInclude(appUserSkill => appUserSkill.Skill).Include(appUserJob => appUserJob.AppUser).ThenInclude(user => user.AppUserCertificates).ThenInclude(appUserCertificate => appUserCertificate.Certificate);

            return Mapping.Map<IEnumerable<AppUserJob>, IEnumerable<ApplicantViewModel>>(applicants);
        }

        public object GetFilters()
        {
            return new
            {
                Industries = EnumHelper.GetDescriptions(typeof(IndustryEnum)),
                Locations = GetLocations()
            };
        }

        public IEnumerable<CompanyViewModel> Filter(CompanyFilterViewModel filter)
        {
            IEnumerable<Company> result = Database.Companies.GetAll().Include(company => company.CompanyAppUsers);

            // sort by location
            if (!string.IsNullOrEmpty(filter.Location))
            {
                result = result.Where(company => !String.IsNullOrEmpty(company.Location) && company.Location.Trim().ToLowerInvariant().Contains(filter.Location.Trim().ToLowerInvariant()));
            }

            // sort by industry
            if (!string.IsNullOrEmpty(filter.Industry))
            {
                result = result.Where(company => company.Industry.GetDescription().Trim().ToLowerInvariant() == filter.Industry.Trim().ToLowerInvariant());
            }

            // sort by company size
            if (filter.MinCompanySize != 0 || filter.MaxCompanySize != 0)
            {
                if (filter.MaxCompanySize == 0)
                {
                    // select from min to infinity
                    result = result.Where(company =>
                        company.CompanyAppUsers.Count(companyAppUser => companyAppUser.CompanyId == company.Id) >= filter.MinCompanySize);
                }
                else
                {
                    // select from min to max
                    result = result.Where(company =>
                        company.CompanyAppUsers.Count(companyAppUser => companyAppUser.CompanyId == company.Id) >= filter.MinCompanySize &&
                        company.CompanyAppUsers.Count(companyAppUser => companyAppUser.CompanyId == company.Id) <= filter.MaxCompanySize);
                }
            }

            return Mapping.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(result);
        }

        public List<string> GetLocations()
        {
            return Database.Companies.GetAll().Where(company => !String.IsNullOrEmpty(company.Location)).Select(company => company.Location).Distinct().ToList();
        }
    }
}
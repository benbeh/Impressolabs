using System.Collections.Generic;
using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;

namespace BLL.Interfaces
{
    public interface ICompanyService : IService<Company, CompanyViewModel>
    {
        IEnumerable<CompanyViewModel> GetAllByName(string name);

        void AddUser(CompanyAppUserViewModel companyAppUser);

        CompanyViewModel GetCompany(int companyId);

        CompanyViewModel GetCurrentUserCompany(string userId);

        IEnumerable<ApplicantViewModel> GetAllApplicants(int companyId);

        object GetFilters();

        IEnumerable<CompanyViewModel> Filter(CompanyFilterViewModel filter);

        List<string> GetLocations();
    }
}
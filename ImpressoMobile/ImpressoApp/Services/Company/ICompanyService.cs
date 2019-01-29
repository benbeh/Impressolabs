using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Models.Company;
using ImpressoApp.Models.Feeds;
namespace ImpressoApp.Services.Company
{
    public interface ICompanyService
    {
        Task<List<CompanyModel>> GetCompanies(CompanyFilterModel name = null);
        Task<CompanyModel> GetCompanyModelAsync(int companyId);
        Task<CompanyFiltersServerModel> GetFilters();
    }
}

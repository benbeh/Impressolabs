using System;
using System.Collections.Generic;
using System.Web;
using ImpressoApp.Models.Feeds;
using ImpressoApp.Services.RequestProvider;
using ImpressoApp.Models.Company;
using ImpressoApp.Constants;
using System.Threading.Tasks;
using System.Linq;
using ImpressoApp.Utils;
using ImpressoApp.Extensions;

namespace ImpressoApp.Services.Company
{
    public class CompanyService : ICompanyService
    {
        private static readonly string ListCompaniesEndpoint = ApplicationConstants.LiveServerApi + "/api/Company/ListSortedCompanies/";
        private static readonly string GetCompanyEndpoint = ApplicationConstants.LiveServerApi + "/api/Company/GetCompany/";
        private static readonly string FiltersEndpoint = ApplicationConstants.LiveServerApi + "/api/Company/GetFilters/";

        private readonly IRequestProvider requestProvider;

        public CompanyService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async System.Threading.Tasks.Task<List<CompanyModel>> GetCompanies(CompanyFilterModel filter = null)
        {
            var filters = new List<ReqestParameter>();

            if (filter?.Location != null)
            {
                filters.Add(new ReqestParameter { Name = "Location", Value = filter.Location });
            }

            if (filter?.Industry != null)
            {
                filters.Add(new ReqestParameter { Name = "Industry", Value = filter.Industry.DescriptionAttr() });
            }

            return await requestProvider.GetAsync<List<CompanyModel>>(ListCompaniesEndpoint, filters);
        }

        public async Task<CompanyModel> GetCompanyModelAsync(int companyId)
        {
            return await requestProvider.GetAsync<CompanyModel>(GetCompanyEndpoint, new List<ReqestParameter> { new ReqestParameter() { Name = "Id", Value = companyId.ToString() } });
        }

        public async Task<CompanyFiltersServerModel> GetFilters()
        {
            var filters = await requestProvider.GetAsync<CompanyFiltersServerModel>(FiltersEndpoint);
            filters.Locations = filters.Locations.Where(l => l != null).ToList();
            return filters;
        }
    }
}

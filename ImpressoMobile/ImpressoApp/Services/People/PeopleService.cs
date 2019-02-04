using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Constants;
using ImpressoApp.Models.Job;
using ImpressoApp.Models.People;
using ImpressoApp.Services.RequestProvider;

namespace ImpressoApp.Services.People
{
    public class PeopleService : IPeopleService
    {
        private const string ListPeopleApi = ApplicationConstants.LiveServerApi + "/api/User/ListSortedUsers";
        private const string GetFiltersApi = ApplicationConstants.LiveServerApi + "/api/User/GetFilters";

        private readonly IRequestProvider requestProvider;

        public PeopleService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<PeopleFiltersServerModel> GetFilters()
        {
            return await requestProvider.GetAsync<PeopleFiltersServerModel>(GetFiltersApi);
        }

        public async Task<List<PeopleSearchModel>> GetFilteredPeople(PeopleFilterModel filterModel)
        {
            var filters = new List<ReqestParameter>();

            if (filterModel.Location != null)
            {
                filters.Add(new ReqestParameter { Name = "Location", Value = filterModel.Location });
            }

            return await requestProvider.GetAsync<List<PeopleSearchModel>>(ListPeopleApi, filters);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Models.Job;
using ImpressoApp.Models.People;

namespace ImpressoApp.Services.People
{
    public interface IPeopleService
    {
        Task<List<PeopleSearchModel>> GetFilteredPeople(PeopleFilterModel filterModel);

        Task<PeopleFiltersServerModel> GetFilters();
    }
}

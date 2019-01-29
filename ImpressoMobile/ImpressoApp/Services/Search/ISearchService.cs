using System;
using ImpressoApp.Models.Job;
using ImpressoApp.Models.Company;
using ImpressoApp.Models.People;

namespace ImpressoApp.Services.Search
{
    public interface ISearchService
    {
        JobFilterModel CurrentJobFilter { get; set; }

        CompanyFilterModel CurrentCompanyFilter { get; set; }

        PeopleFilterModel CurrentPeopleFilter { get; set; }
    }
}

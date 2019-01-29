using System;
using ImpressoApp.Models.Job;
using ImpressoApp.Models.Company;
using ImpressoApp.Models.People;

namespace ImpressoApp.Services.Search
{
    public class SearchService : ISearchService
    {
        public JobFilterModel CurrentJobFilter { get; set; }

        public CompanyFilterModel CurrentCompanyFilter { get; set; }

        public PeopleFilterModel CurrentPeopleFilter { get; set; }

        public SearchService()
        {
            CurrentJobFilter = new JobFilterModel();
            CurrentCompanyFilter = new CompanyFilterModel();
            CurrentPeopleFilter = new PeopleFilterModel();
        }
    }
}

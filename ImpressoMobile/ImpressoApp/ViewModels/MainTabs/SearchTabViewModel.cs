using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ImpressoApp.Controls;
using Xamarin.Forms;
using ImpressoApp.Services.Job;
using ImpressoApp.Models.Job;
using System.Threading.Tasks;
using ImpressoApp.Models.Feeds;
using ImpressoApp.Models.Company;
using System.Collections.Generic;
using System.Linq;
using ImpressoApp.Utils;
using ImpressoApp.Services.Company;
using ImpressoApp.Services.Search;
using ImpressoApp.Models.People;
using ImpressoApp.Services.People;
using ImpressoApp.Models.Search;
using ImpressoApp.ViewModels.Search;
using ImpressoApp.Enums;
using ImpressoApp.ViewModels.Feeds;
using ImpressoApp.Extensions;
using ImpressoApp.ViewModels.Profile;

namespace ImpressoApp.ViewModels.MainTabs
{
    public class SearchTabViewModel : BaseViewModel
    {
        public const string MostRecent = "Most Recent";
        public const string MostRelevant = "Most Relevant";

        IJobService jobService;
        ICompanyService companyService;
        ISearchService searchService;
        IPeopleService peopleService;

        public ICommand TabChangedCommand { get; private set; }
        public ICommand SearchTextChangedCommand { get; private set; }
        public ICommand SearchCompletedCommand { get; private set; }
        public ICommand ShowFiltersCommand { get; private set; }
        public ICommand JobsItemSelectedCommand { get; private set; }
        public ICommand CompanyItemSelectedCommand { get; private set; }
        public ICommand PeopleItemSelectedCommand { get; private set; }

        private List<JobModel> currentJobs;
        private List<CompanyModel> currentCompanies;
        private List<PeopleSearchModel> currenPeople;

        private List<string> locations;

        public bool FirstTabActive { get; set; } = true;
        public bool SecondTabActive { get; set; }
        public bool ThirdTabActive { get; set; }

        public ObservableCollection<JobModel> JobsList { get; set; }
        public ObservableCollection<CompanyModel> CompanyList { get; set; }
        public ObservableCollection<PeopleSearchModel> PeopleList { get; set; }

        public ObservableCollection<TabIndicatorModel> Tabs { get; set; }

        public string SearchPlaceholder { get; set; }

        public string SearchText { get; set; }

        public SearchTabViewModel(INavigationService navigationService, 
                                  IJobService jobService, 
                                  ICompanyService companyService, 
                                  ISearchService searchService, 
                                  IPeopleService peopleService) : base(navigationService)
        {
            Title = "Search";
            Icon = "searchOutline";

            TabChangedCommand = new Command(TabSelectedCommandExecute);
            SearchTextChangedCommand = new Command(OnSearchTextChanged);
            SearchCompletedCommand = new Command(OnSearchCompleted);
            ShowFiltersCommand = new Command(OnShowFilters);
            JobsItemSelectedCommand = new Command(JobsItemSelectedCommandExecute);
            CompanyItemSelectedCommand = new Command(CompanyItemSelectedCommandExecute);
            PeopleItemSelectedCommand = new Command(PeopleItemSelectedCommandExecute);

            this.jobService = jobService;
            this.companyService = companyService;
            this.searchService = searchService;
            this.peopleService = peopleService;

            SetTabs();
        }

        private async void PeopleItemSelectedCommandExecute(object obj)
        {
            if(obj is PeopleSearchModel peopleModel)
            {
                await NavigationService.NavigateToAsync<ViewProfileViewModel>(peopleModel.Id);
            }
        }

        private async void CompanyItemSelectedCommandExecute(object obj)
        {
            var model = obj as CompanyModel;
            await NavigationService.NavigateToAsync<AboutCompanyViewModel>(model.ID);
        }

        private async void JobsItemSelectedCommandExecute(object obj)
        {
            var model = obj as JobModel;
            await NavigationService.NavigateToAsync<JobPostViewModel>(model);
        }

        private void SetTabs()
        {
            Tabs = new ObservableCollection<TabIndicatorModel>();

            if (!IsCompany)
            {
                Tabs.Add(new TabIndicatorModel { Title = "Jobs" });
                Tabs.Add(new TabIndicatorModel { Title = "Companies" });
            }
            Tabs.Add(new TabIndicatorModel { Title = "People" });
        }

        public async override void OnStart()
        {
            base.OnStart();

            TabSelectedCommandExecute(0);
        }

        public async override void OnAppearing()
        {
            base.OnAppearing();

            if (FirstTabActive)
            {
                await UpdateJobsWithFilter();
            }
            if (SecondTabActive)
            {
                await UpdateCompaniesWithFilter();
            }
            if (ThirdTabActive)
            {
                await UpdatePeopleWithFilter();
            }
        }

        void OnSearchTextChanged(object obj)
        {
            if (FirstTabActive)
            {
                UpdateJobsWithSearchFilter();
            }
            if (SecondTabActive)
            {
                UpdateCompaniesWithSearchFilter();
            }
            if (ThirdTabActive)
            {
                UpdatePeopleWithSearchFilter();
            }
        }

        async void OnShowFilters(object obj)
        {
            var searchFilters = new List<SearchFilterItemModel>();

            if (FirstTabActive)
            {
                var filters = await jobService.GetFilters();
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "SORT BY",
                    IsSearchAvailable = false,
                    SearchItems = new List<string>() { MostRecent, MostRelevant },
                    SearchType = SearchFilterItem.CheckmarkSingleChoise,
                    CommitAction = (value) => { searchService.CurrentJobFilter.IsMostRelevant = value[0] == MostRelevant; },
                    GetAction = () => new List<string>() { searchService.CurrentJobFilter.IsMostRelevant ? MostRelevant : MostRecent }
                });
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "COMPANY",
                    SearchItems = filters.Companies,
                    SearchType = SearchFilterItem.CheckmarkSingleChoise,
                    CommitAction = (value) => { searchService.CurrentJobFilter.CompanyName = value[0]; },
                    GetAction = () => new List<string>() { searchService.CurrentJobFilter.CompanyName }
                });
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "JOB TYPE",
                    SearchItems = filters.JobTypes,
                    SearchType = SearchFilterItem.CheckmarkMultipleChoise,
                    CommitAction = (value) => { searchService.CurrentJobFilter.JobTypes = value[0] != null ? value : null; },
                    GetAction = () => searchService.CurrentJobFilter.JobTypes
                });
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "SKILLS",
                    SearchItems = filters.Skills,
                    SearchType = SearchFilterItem.CheckmarkSingleChoise,
                    CommitAction = (value) => { searchService.CurrentJobFilter.Skills = value[0] != null ? value : null; },
                    GetAction = () => searchService.CurrentJobFilter.Skills
                });
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "EXPERIENCE",
                    SearchItems = filters.Experience,
                    SearchType = SearchFilterItem.CheckmarkSingleChoise,
                    CommitAction = (value) => { searchService.CurrentJobFilter.Experience = value[0] != null ? value : null; },
                    GetAction = () => searchService.CurrentJobFilter.Experience
                });
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "INDUSTRY",
                    SearchItems = filters.Industries,
                    SearchType = SearchFilterItem.CheckmarkSingleChoise,
                    CommitAction = (value) => { searchService.CurrentJobFilter.Industry = value[0]; },
                    GetAction = () => new List<string>() { searchService.CurrentJobFilter.Industry }
                });
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "CERTIFICATES",
                    SearchItems = filters.Certificates,
                    SearchType = SearchFilterItem.CheckmarkMultipleChoise,
                    CommitAction = (value) => { searchService.CurrentJobFilter.Certificates = value[0] != null ? value : null; },
                    GetAction = () => searchService.CurrentJobFilter.Certificates
                });
            }
            else if (SecondTabActive)
            {
                var filters = await companyService.GetFilters();
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "LOCATION",
                    SearchItems = filters.Locations,
                    SearchType = SearchFilterItem.CheckmarkSingleChoise,
                    GetAction = () => new List<string>() { searchService.CurrentCompanyFilter.Location },
                    CommitAction = (value) => searchService.CurrentCompanyFilter.Location = value[0]
                });
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "INDUSTRY",
                    SearchItems = filters.Industries,
                    SearchType = SearchFilterItem.CheckmarkSingleChoise,
                    GetAction = () => new List<string>() { searchService.CurrentCompanyFilter.Industry.DescriptionAttr() },
                    CommitAction = (value) => searchService.CurrentCompanyFilter.Industry = value[0] == null ? Industry.None : value[0].GetEnumValueFromDescription<Industry>()
                });
                //searchFilters.Add(new SearchFilterItemModel() { Name = "COMPANY SIZE", SearchItems = currentCompanies.Select(x => x.Location).Distinct().ToList(), SearchType = SearchFilterItem.CheckmarkSingleChoise, GetAction = () => new List<string>() { searchService.CurrentCompanyFilter.Size }, CommitAction = (value) => searchService.CurrentCompanyFilter.Location = value[0] });
            }
            else if (ThirdTabActive)
            {
                var filters = await peopleService.GetFilters();
                searchFilters.Add(new SearchFilterItemModel()
                {
                    Name = "LOCATION",
                    SearchItems = filters.Locations,
                    SearchType = SearchFilterItem.CheckmarkSingleChoise,
                    GetAction = () => new List<string>() { searchService.CurrentPeopleFilter.Location },
                    CommitAction = (value) => searchService.CurrentPeopleFilter.Location = value[0]
                });
                //searchFilters.Add(new SearchFilterItemModel() { Name = "COMPANY SIZE", SearchItems = currentCompanies.Select(x => x.Location).Distinct().ToList(), SearchType = SearchFilterItem.CheckmarkSingleChoise, GetAction = () => new List<string>() { searchService.CurrentCompanyFilter.Size }, CommitAction = (value) => searchService.CurrentCompanyFilter.Location = value[0] });
            }

            await NavigationService.NavigateToAsync<FilterMainViewModel>(searchFilters);
        }

        void OnSearchCompleted(object obj)
        {
            if (FirstTabActive)
            {
                UpdateJobsWithSearchFilter();
            }
            if (SecondTabActive)
            {
                UpdateCompaniesWithSearchFilter();
            }
            if (ThirdTabActive)
            {
                UpdatePeopleWithSearchFilter();
            }
        }

        private async Task UpdateJobsWithFilter()
        {
            IsBusy = true;
            try
            {
                currentJobs = await jobService.GetFilteredJobs(searchService.CurrentJobFilter);

                UpdateJobsWithSearchFilter();
            }
            catch (Exception ex)
            {
                //TODO
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdateCompaniesWithFilter()
        {
            IsBusy = true;
            try
            {
                currentCompanies = await companyService.GetCompanies(searchService.CurrentCompanyFilter);
                CompanyList = new ObservableCollection<CompanyModel>(currentCompanies);
            }
            catch (Exception ex)
            {
                //TODO
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdatePeopleWithFilter()
        {
            IsBusy = true;
            try
            {
                currenPeople = await peopleService.GetFilteredPeople(searchService.CurrentPeopleFilter);
                PeopleList = new ObservableCollection<PeopleSearchModel>(currenPeople);
            }
            catch (Exception ex)
            {
                //TODO
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateJobsWithSearchFilter()
        {
            if (currentJobs == null)
            {
                return;
            }
            JobsList = new ObservableCollection<JobModel>(string.IsNullOrEmpty(SearchText) ? currentJobs : currentJobs.Where(job => job.Title.ContainsIgnoreCase(SearchText)));
        }

        private void UpdateCompaniesWithSearchFilter()
        {
            if (currentCompanies == null)
            {
                return;
            }
            CompanyList = new ObservableCollection<CompanyModel>(string.IsNullOrEmpty(SearchText) ? currentCompanies : currentCompanies.Where(company => company.CompanyName.ContainsIgnoreCase(SearchText)));
        }

        private void UpdatePeopleWithSearchFilter()
        {
            if (currenPeople == null)
            {
                return;
            }
            PeopleList = new ObservableCollection<PeopleSearchModel>(string.IsNullOrEmpty(SearchText) ? currenPeople : currenPeople.Where(p => p.UserName.ContainsIgnoreCase(SearchText)));
        }

        private async void TabSelectedCommandExecute(object obj)
        {
            SearchText = string.Empty;

            var tabIndex = IsCompany ? 2 : (int)obj;

            FirstTabActive = false;
            SecondTabActive = false;
            ThirdTabActive = false;

            SearchPlaceholder = "Search";

            switch (tabIndex)
            {
                case 0:
                    FirstTabActive = true;
                    await UpdateJobsWithFilter();
                    break;
                case 1:
                    SecondTabActive = true;
                    await UpdateCompaniesWithFilter();
                    locations = currentCompanies.Select(x => x.Location).Distinct().ToList();
                    break;
                case 2:
                    ThirdTabActive = true;
                    await UpdatePeopleWithFilter();
                    break;
                default:
                    FirstTabActive = true;
                    break;
            }
        }


    }
}

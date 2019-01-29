using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using ImpressoApp.Models.Job;
using ImpressoApp.Models.Company;
using ImpressoApp.Services.Company;
using ImpressoApp.Exceptions;

namespace ImpressoApp.ViewModels.Feeds
{
    public class AboutCompanyViewModel : BaseViewModel
    {
        private readonly ICompanyService companyService;

        private int companyId;

        public bool IsLoaded { get; private set; }
        public CompanyModel CompanyModel { get; set; }
        public ICommand VacancyTapCommand { get; private set; }

        public AboutCompanyViewModel(INavigationService navigationService, ICompanyService companyService) : base(navigationService)
        {
            this.companyService = companyService;

            VacancyTapCommand = new Command(VacancyTapCommandExecute);
        }

        private async void VacancyTapCommandExecute(object obj)
        {
            var vacancyModel = obj as JobModel;

            if (vacancyModel != null)
            {
                await NavigationService.NavigateToAsync<JobPostViewModel>(vacancyModel);
            }
        }

        public override Task Init(object args)
        {
            if(args is int companyId)
            {
                this.companyId = companyId;
            }

            return base.Init(args);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            LoadCompany();
        }

        private async void LoadCompany()
        {
            try
            {
                IsBusy = true;
                var par = companyId.ToString();
                var result = await companyService.GetCompanyModelAsync(companyId);

                if (result.IsSuccess)
                {
                    CompanyModel = result;
                    IsLoaded = true;

                    Title = string.Format("{0} Company", CompanyModel.CompanyName);
                }
                else
                {
                    await NavigationService.DisplayAlert("Error", result.Message, "Ok");
                }
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await NavigationService.DisplayAlert("Error", "Error loading company information. Check internet connection and try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

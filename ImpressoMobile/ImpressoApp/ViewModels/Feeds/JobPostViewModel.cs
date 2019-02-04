using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.Models.Job;
using ImpressoApp.Services.Job;
using ImpressoApp.Exceptions;
using System.Threading;
using ImpressoApp.Models.User;
using ImpressoApp.Services.Company;

namespace ImpressoApp.ViewModels.Feeds
{
    public class JobPostViewModel : BaseViewModel
    {
        private readonly IJobService jobService;

        public JobModel JobPostModel { get; set; }

        public ICommand AboutCompanyCommand { get; private set; }
        public ICommand OpenOtherPositionsCommand { get; private set; }
        public ICommand ApplyJobCommand { get; private set; }
        public ICommand CancelJobDialogCommand { get; private set; }
        public ICommand ApplyJobWithAccountCommand { get; private set; }
        public ICommand BookmarkJobCommand { get; private set; }

        public JobPostViewModel(INavigationService navigationService,
                                IJobService jobService) : base(navigationService)
        {
            this.jobService = jobService;

            Title = "Job Post";

            AboutCompanyCommand = new Command(AboutCompanyCommandExecute);
            ApplyJobCommand = new Command(ApplyJobCommandExecute);
            CancelJobDialogCommand = new Command(CancelJobDialogCommandExecute);
            ApplyJobWithAccountCommand = new Command(ApplyJobWithAccountCommandExecute);
            OpenOtherPositionsCommand = new Command(OpenOtherPositionsCommandExecute);
            BookmarkJobCommand = new Command(BookmarkJobCommandExecute);
        }

        private async void BookmarkJobCommandExecute(object obj)
        {
            try
            {
                var result = await jobService.SetAsBookmarkedAsync(new SetAsBookmarkedRequestModel
                {
                    Id = JobPostModel.ID.ToString(),
                    IsBookmarked = JobPostModel.IsBookmarked
                });

                JobPostModel.IsBookmarked = !JobPostModel.IsBookmarked;

                if (!result.IsSuccess)
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
                await NavigationService.DisplayAlert("Error", "Error bookmarking this job. Check internet connection and try again.", "Ok");
            }
        }

        private async void OpenOtherPositionsCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<AboutCompanyViewModel>(JobPostModel.CompanyId);
        }

        private async void ApplyJobWithAccountCommandExecute(object obj)
        {
            try
            {
                IsBusy = true;

                var result = await jobService.ApplyForJobAsync(JobPostModel.ID);

                if (result.IsSuccess)
                {
                    JobPostModel.IsApplied = true;

                    await NavigationService.HideDialog();

                    await NavigationService.DisplayDialog(new CongratulationDialogViewModel(NavigationService));
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
                await NavigationService.DisplayAlert("Error", "Apply job error. Check internet connection and try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CancelJobDialogCommandExecute(object obj)
        {
            NavigationService.HideDialog();
        }

        private void ApplyJobCommandExecute(object obj)
        {
            var applyJobDialogVM = new ApplyJobDialogViewModel();
            applyJobDialogVM.JobModel = JobPostModel;
            applyJobDialogVM.CancelCommand = CancelJobDialogCommand;
            applyJobDialogVM.ApplyWithYourProfileCommand = ApplyJobWithAccountCommand;
            NavigationService.DisplayDialog(applyJobDialogVM);
        }

        private async void AboutCompanyCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<AboutCompanyViewModel>(JobPostModel.CompanyId);
        }

        private async void LoadJobInfo(int companyId)
        {
            try
            {
                IsBusy = true;

                JobPostModel = await jobService.GetJobInfo(companyId.ToString());
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await NavigationService.DisplayAlert("Error", "Apply job error. Check internet connection and try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override Task Init(object args)
        {
            if (args is JobModel jobModel)
            {
                JobPostModel = jobModel;
            }
            else if (args is int jobId)
            {
                LoadJobInfo(jobId);
            }

            return base.Init(args);
        }
    }
}

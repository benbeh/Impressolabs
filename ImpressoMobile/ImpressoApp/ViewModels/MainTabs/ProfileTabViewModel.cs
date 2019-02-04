using System;
using Xamarin.Forms;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using System.Collections.ObjectModel;
using ImpressoApp.Controls;
using System.Windows.Input;
using ImpressoApp.Services.Profile;
using System.Collections.Generic;
using ImpressoApp.Models.Profile;
using ImpressoApp.Exceptions;
using ImpressoApp.ViewModels.Profile;
using ImpressoApp.Services.Token;
using ImpressoApp.Models.Token;
using ImpressoApp.Services.Job;
using ImpressoApp.Models.Job;
using Java.Nio.Channels;
using ImpressoApp.ViewModels.Feeds;
using ImpressoApp.Services.Project;
using ImpressoApp.Models.Project;
using ImpressoApp.Services.User;
using ImpressoApp.Pages;

namespace ImpressoApp.ViewModels.MainTabs
{
    public class ProfileTabViewModel : BaseViewModel
    {
        private readonly IProfileService profileService;
        private readonly ITokenService tokenService;
        private readonly IJobService jobService;
        private readonly IProjectService projectService;
        private readonly IUserService userService;

        public ObservableCollection<ProjectModel> ProjectList { get; set; }
        public ObservableCollection<TagServerModel> TagsList { get; set; }
        public ObservableCollection<JobModel> JobsList { get; set; }
        public ObservableCollection<TokenServerModel> TokensList { get; set; }

        public ICommand AddJobCommand { get; private set; }
        public ICommand TabChangedCommand { get; private set; }
        public ICommand MoreTagMenuCommand { get; set; }
        public ICommand DeleteTagCommand { get; set; }
        public ICommand NoCommand { get; set; }
        public ICommand ExpandTokenItemCommand { get; private set; }
        public ICommand MenuCommand { get; private set; }
        public ICommand JobTapCommand { get; private set; }
        public ICommand ProjectTapCommand { get; private set; }


        public ObservableCollection<TabIndicatorModel> Tabs { get; set; }

        public bool FirstTabActiveUser { get; set; } = true;
        public bool FirstTabActiveCompany { get; set; } = true;
        public bool SecondTabActive { get; set; }
        public bool ThirdTabActive { get; set; }

        public bool AddJobVisible { get; private set; }

        public bool HasJobs { get; private set; }
        public bool HasTags { get; private set; }
        public bool HasProjects { get; private set; }

        private bool tagsLoaded;
        private bool jobsLoaded;
        private bool tokensLoaded;

        public string UserProfileImagePath { get; set; }
        public string UserName { get; set; }
        public double TotalAmountOfTokens { get; set; }

        public ProfileTabViewModel(INavigationService navigationService,
                                   IProfileService profileService,
                                   ITokenService tokenService,
                                   IJobService jobService, IUserService userService, IProjectService projectService) : base(navigationService)
        {
            this.profileService = profileService;
            this.tokenService = tokenService;
            this.jobService = jobService;
            this.projectService = projectService;
            this.userService = userService;

            Title = "Profile";
            Icon = "profileOutline";

            SetTabs();

            TabChangedCommand = new Command(TabSelectedCommandExecute);
            MoreTagMenuCommand = new Command(MoreTagMenuCommandExecute);
            DeleteTagCommand = new Command(DeleteTagCommandExecute);
            NoCommand = new Command(NoCommandExecute);
            ExpandTokenItemCommand = new Command(ExpandTokenItemCommandExecute);
            MenuCommand = new Command(MenuCommandExecute);
            JobTapCommand = new Command(JobTapCommandExecute);
            AddJobCommand = new Command(CreateJobExecute);
            ProjectTapCommand = new Command(OpenProjectExecute);

            AddJobVisible = navigationService.IsCompany;

            TabSelectedCommandExecute(0);
        }

        private async void CreateJobExecute()
        {
            //await NavigationService.DisplayDialog<NotImplementedDialogViewModel>();
            await NavigationService.DisplayDialog<CreateJobDialogViewModel>();
        }

        private async void OpenProjectExecute(object ob)
        {
            var proj = ob as ProjectModel;
            await NavigationService.NavigateToAsync<ProjectViewModel>(proj.Name);
            //NavigationService.DisplayDialog<CreateJobDialogViewModel>();
        }

        private async void JobTapCommandExecute(object obj)
        {
            if (obj is JobModel jobModel)
            {
                await NavigationService.NavigateToAsync<JobPostViewModel>(jobModel.ID);
            }
        }

        private async void MenuCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<MenuViewModel>();
        }

        private void ExpandTokenItemCommandExecute(object obj)
        {
            if (obj is TokenModel tokenModel)
            {
                tokenModel.IsExpanded = !tokenModel.IsExpanded;
            }
        }

        private void NoCommandExecute(object obj)
        {
            NavigationService.HideDialog();
        }

        private void DeleteTagCommandExecute(object obj)
        {

        }

        private async void MoreTagMenuCommandExecute(object obj)
        {
            //if (obj is TagServerModel model)
            //{

            //    //var dialogVM = new DeleteTagDialogViewModel();
            //    //dialogVM.YesCommand = DeleteTagCommand;
            //    //dialogVM.NoCommand = NoCommand;

            //    //navigationService.DisplayDialog(dialogVM);

            //    var delete = await NavigationService.DisplayAlert("Delete Tag", "", "Yes", "No");
            //    if (delete)
            //    {
            //        TagsList.Remove(model);
            //        TagsList = new ObservableCollection<TagServerModel>(TagsList);
            //    }/api/User/GetUserProfileInfoById
            //}

            await NavigationService.DisplayDialog(new NotImplementedDialogViewModel());
        }

        private async void TabSelectedCommandExecute(object obj)
        {
            var tabIndex = (int)obj;

            FirstTabActiveCompany = false;
            FirstTabActiveUser = false;
            SecondTabActive = false;
            ThirdTabActive = false;

            switch (tabIndex)
            {
                case 0:
                    if (IsCompany)
                    {
                        FirstTabActiveCompany = true;
                        LoadProjects();
                    }
                    else
                    {
                        FirstTabActiveUser = true;
                        LoadTags();
                    }
                    break;
                case 1:
                    SecondTabActive = true;
                    LoadJobs();
                    break;
                case 2:
                    ThirdTabActive = true;
                    LoadTokens();
                    break;
                default:
                    FirstTabActiveUser = true;
                    break;
            }
        }

        private void SetTabs()
        {
            Tabs = new ObservableCollection<TabIndicatorModel>();
            Tabs.Add(new TabIndicatorModel { Title = IsCompany ? "Projects" : "Tags" });
            Tabs.Add(new TabIndicatorModel { Title = "Jobs" });
            Tabs.Add(new TabIndicatorModel { Title = "Tokens" });
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            UserProfileImagePath = Settings.UserProfileImage;
            UserName = Settings.UserName;

            LoadTags();
        }

        private async void LoadTags()
        {
            try
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(Settings.Token))
                {
                    var tags = await profileService.GetTagsAsync();
                    TagsList = new ObservableCollection<TagServerModel>(tags);

                    HasTags = TagsList.Count > 0;

                    tagsLoaded = true;
                }
            }
            catch (ServiceAuthenticationException e)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", string.Empty, "Close");
            }
            catch (Exception e)
            {
                await NavigationService.DisplayAlert("Error", "Sign up error. Check internet connection and try again.", string.Empty, "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadJobs()
        {
            try
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(Settings.Token))
                {
                    var jobs = IsCompany ? (await userService.GetCurrentCompanyInfo()).Jobs : await jobService.GetAppliedJobsAsync();
                    JobsList = new ObservableCollection<JobModel>(jobs);
                    HasJobs = JobsList.Count > 0;

                    jobsLoaded = true;
                }
            }
            catch (ServiceAuthenticationException e)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", string.Empty, "Close");
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Error getting applied jobs. Check internet connection and try again.", string.Empty, "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadTokens()
        {
            if (tokensLoaded)
            {
                return;
            }

            try
            {
                IsBusy = true;

                TotalAmountOfTokens = await tokenService.GetTotalTokensAmount();

                var tokens = await tokenService.GetListTokens();
                TokensList = new ObservableCollection<TokenServerModel>(tokens);

                TotalAmountOfTokens = await tokenService.GetTotalTokensAmount();

                tokensLoaded = true;
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", string.Empty, "Close");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await NavigationService.DisplayAlert("Error", "Sign up error. Check internet connection and try again.", string.Empty, "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadProjects()
        {
            try
            {
                IsBusy = true;

                var projects = (await userService.GetCurrentCompanyInfo()).Projects;
                ProjectList = new ObservableCollection<ProjectModel>(projects);
                HasProjects = ProjectList.Count > 0;
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", string.Empty, "Close");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await NavigationService.DisplayAlert("Error", "Sign up error. Check internet connection and try again.", string.Empty, "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

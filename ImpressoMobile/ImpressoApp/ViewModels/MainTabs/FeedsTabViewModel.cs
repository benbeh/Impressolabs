using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using ImpressoApp.Exceptions;
using ImpressoApp.Services.Feeds;
using ImpressoApp.Controls;
using ImpressoApp.Models.Feeds;
using ImpressoApp.ViewModels.Feeds;
using ImpressoApp.Models.Job;
using ImpressoApp.Services.Job;
using ImpressoApp.Services.Event;
using ImpressoApp.Services.User;
using ImpressoApp.Models.User;
using ImpressoApp.Models.People;
using ImpressoApp.ViewModels.Profile;

namespace ImpressoApp.ViewModels.MainTabs
{
    public class FeedsTabViewModel : BaseViewModel
    {
        private readonly IFeedsService feedsService;
        private readonly IJobService jobService;
        private readonly IEventService eventService;
        private readonly IUserService userService;

        private bool isInitialized = false;

        public ICommand JobsItemSelectedCommand { get; private set; }
        public ICommand TabChangedCommand { get; private set; }
        public ICommand EventTapCommand { get; private set; }
        public ICommand ConnectTapCommand { get; private set; }
        public ICommand ConnectToPersonCommand { get; private set; }
        public ICommand ApplyJobCommand { get; private set; }
        public ICommand InterestedEventCommand { get; private set; }
        public ICommand PeopleBookmarkCommand { get; private set; }
        public ICommand JobBookmarkCommand { get; private set; }
        public ICommand PeopleItemTapCommand { get; private set; }
        public ICommand RecommendJobCommand { get; private set; }
        public ICommand RecommandPeopleCommand { get; private set; }
        public ICommand ShareEventCommand { get; private set; }

        public ObservableCollection<JobModel> JobsList { get; set; }
        public ObservableCollection<IConnectFeedModel> ConnectsList { get; set; }

        public ObservableCollection<TabIndicatorModel> Tabs { get; set; }

        public bool FirstTabActive { get; set; } = true;
        public bool SecondTabActive { get; set; }

        public JobModel SelectedJob { get; set; }

        public FeedsTabViewModel(INavigationService navigationService,
                                 IFeedsService feedsService,
                                 IJobService jobService,
                                 IEventService eventService,
                                 IUserService userService) : base(navigationService)
        {
            Title = "Feeds";
            Icon = "feedsOutline.png";

            SetTabs();

            this.feedsService = feedsService;
            this.jobService = jobService;
            this.eventService = eventService;
            this.userService = userService;

            JobsItemSelectedCommand = new Command(JobsItemSelectedCommandExecute);
            TabChangedCommand = new Command(TabSelectedCommandExecute);
            ConnectTapCommand = new Command(ConnectTapCommandExecute);
            EventTapCommand = new Command(EventTapCommandExecute);
            ApplyJobCommand = new Command(ApplyJobCommandExecute);
            InterestedEventCommand = new Command(InterestedEventCommandExecute);
            PeopleBookmarkCommand = new Command(PeopleBookmarkCommandExecute);
            JobBookmarkCommand = new Command(JobBookmarkCommandExecute);
            PeopleItemTapCommand = new Command(PeopleItemTapCommandExecute);
            RecommendJobCommand = new Command(RecommendJobCommandExecute);
            RecommandPeopleCommand = new Command(RecommandPeopleCommandExecute);
            ShareEventCommand = new Command(ShareEventCommandExecute);
        }

        private async void ShareEventCommandExecute(object obj)
        {
            await NavigationService.DisplayDialog(new NotImplementedDialogViewModel());
        }

        private async void RecommandPeopleCommandExecute(object obj)
        {
            await NavigationService.DisplayDialog(new NotImplementedDialogViewModel());
        }

        public async override void OnStart()
        {
            base.OnStart();

            TabSelectedCommandExecute(0);

            if (!isInitialized)
            {
                try
                {
                    IsBusy = true;

                    var ethereumAddress = await userService.GetCurrentUserEthereumAddressAsync();
                    if (ethereumAddress == null)
                    {
                        if (Settings.DoNotShowAgainEthereumDialogEmail != Settings.UserName)
                        {
                            await NavigationService.DisplayDialog<EtherumAddressDialogViewModel>();
                        }
                    }
                }
                catch (ServiceAuthenticationException)
                {
                    await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", string.Empty, "Close");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await NavigationService.DisplayAlert("Error", "Unknown issue. Please try again", string.Empty, "Close");
                }
                finally
                {
                    IsBusy = false;
                }
            }

            isInitialized = true;
        }

        private async void RecommendJobCommandExecute(object obj)
        {
            await NavigationService.DisplayDialog(new NotImplementedDialogViewModel());
        }

        private async void PeopleItemTapCommandExecute(object obj)
        {
            if (obj is ConnectPeopleModel peopleModel)
            {
                await NavigationService.NavigateToAsync<ViewProfileViewModel>(peopleModel.ID);
            }
        }

        private async void JobBookmarkCommandExecute(object obj)
        {
            if (obj is JobModel model)
            {
                try
                {
                    model.IsBookmarked = !model.IsBookmarked;

                    var result = await jobService.SetAsBookmarkedAsync(new SetAsBookmarkedRequestModel
                    {
                        Id = model.ID.ToString(),
                        IsBookmarked = model.IsBookmarked
                    });
                }
                catch (ServiceAuthenticationException)
                {
                    await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await NavigationService.DisplayAlert("Error", "Sign up error. Check internet connection and try again.", "Close");
                }
            }
        }

        private async void PeopleBookmarkCommandExecute(object obj)
        {
            if (obj is ConnectPeopleModel model)
            {
                try
                {
                    model.IsBookmarked = !model.IsBookmarked;

                    var result = await userService.SetAsBookmarkedAsync(new SetAsBookmarkedRequestModel
                    {
                        Id = model.ID,
                        IsBookmarked = model.IsBookmarked
                    });

                    if (!result.IsSuccess)
                    {
                        model.IsBookmarked = !model.IsBookmarked;
                    }
                }
                catch (ServiceAuthenticationException)
                {
                    await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await NavigationService.DisplayAlert("Error", "Sign up error. Check internet connection and try again.", "Close");
                }
            }
        }

        private async void InterestedEventCommandExecute(object obj)
        {
            if (obj is ConnectEventModel model)
            {
                try
                {
                    var result = await eventService.SetAsInterestedAsync(model.Id.ToString());

                    if (result.IsSuccess)
                    {
                        model.IsInterested = true;
                    }
                }
                catch (ServiceAuthenticationException)
                {
                    await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await NavigationService.DisplayAlert("Error", "Check internet connection and try again.", "Ok");
                }
            }
        }

        private async void ApplyJobCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<JobPostViewModel>(obj);
        }

        private async void EventTapCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<EventPostViewModel>(obj);
        }

        private async void ConnectTapCommandExecute(object obj)
        {
            //if (obj is UserModel user)
            //{
            //    var vm = new ConnectToPersonDialogViewModel();
            //    vm.UserModel = user;
            //    await NavigationService.DisplayDialog(vm);
            //}
            if (obj is ConnectPeopleModel model)
            {
                await NavigationService.NavigateToAsync<ViewProfileViewModel>(model.ID);
            }
        }

        private async void TabSelectedCommandExecute(object obj)
        {
            var tabIndex = IsCompany ? 1 : (int)obj;

            FirstTabActive = false;
            SecondTabActive = false;

            switch (tabIndex)
            {
                case 0:
                    FirstTabActive = true;
                    break;
                case 1:
                    SecondTabActive = true;
                    await GetConnectsList();
                    break;
                default:
                    FirstTabActive = true;
                    break;
            }
        }


        private void JobsItemSelectedCommandExecute(object obj)
        {
            var model = obj as JobModel;
        }

        public async override void OnAppearing()
        {
            base.OnAppearing();

            await GetJobsList();
        }

        private void SetTabs()
        {
            Tabs = new ObservableCollection<TabIndicatorModel>();
            Tabs.Add(new TabIndicatorModel { Title = "Jobs" });
            Tabs.Add(new TabIndicatorModel { Title = "Connect" });
        }

        public async Task GetJobsList()
        {
            try
            {
                IsBusy = true;
                if (!string.IsNullOrEmpty(Settings.Token))
                {
                    var jobs = await jobService.GetJobs();
                    JobsList = new ObservableCollection<JobModel>(jobs);
                }
            }
            catch (ServiceAuthenticationException e)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Sign up error. Check internet connection and try again.", "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetConnectsList()
        {
            try
            {
                IsBusy = true;
                //await Task.Delay(3000);
                var connects = await feedsService.GetConnects();
                ConnectsList = new ObservableCollection<IConnectFeedModel>(connects);
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
            }
            catch (Exception)
            {
                await NavigationService.DisplayAlert("Error", "Sign up error. Check internet connection and try again.", "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

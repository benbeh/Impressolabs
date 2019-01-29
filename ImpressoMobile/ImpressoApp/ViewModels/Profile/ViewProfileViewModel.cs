using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using ImpressoApp.Controls;
using System.Threading.Tasks;
using ImpressoApp.Services.User;
using ImpressoApp.Models.User;
using ImpressoApp.Exceptions;
using ImpressoApp.ViewModels.Testimonials;
using ImpressoApp.Models.Testimonial;

namespace ImpressoApp.ViewModels.Profile
{
    public class ViewProfileViewModel : BaseViewModel
    {
        private readonly IUserService userService;

        public bool IsLoaded { get; private set; }
        public bool IsEditMode { get; private set; }

        public UserModel UserModel { get; private set; }

        public ObservableCollection<TabIndicatorModel> Tabs { get; set; }
        public bool FirstTabActive { get; set; } = true;
        public bool SecondTabActive { get; set; }
        public bool ThirdTabActive { get; set; }

        private string userId;

        public ICommand TabChangedCommand => new Command(TabSelectedCommandExecute);
        public ICommand RecommendCommand => new Command(RecommendCommandExecute);
        public ICommand ConnectCommand => new Command(ConnectCommandExecute);
        public ICommand ViewAllTestimonialsCommand => new Command(ViewAllTestimonialsCommandExecute);
        public ICommand SelectTestimonialCommand => new Command(SelectTestimonialCommandExecute);
        public ICommand OpenCVLinkCommand => new Command(OpenCVLinkCommandExecute);
        public ICommand BookmarkCommand => new Command(BookmarkCommandExecute);

        public ViewProfileViewModel(INavigationService navigationService,
                                    IUserService userService) : base(navigationService)
        {
            this.userService = userService;

            Title = "Profile";

            SetTabs();
        }

        private async void BookmarkCommandExecute(object obj)
        {
            try
            {
                UserModel.IsBookmarked = !UserModel.IsBookmarked;

                var result = await userService.SetAsBookmarkedAsync(new SetAsBookmarkedRequestModel
                {
                    Id = UserModel.Id,
                    IsBookmarked = UserModel.IsBookmarked
                });

                if (!result.IsSuccess)
                {
                    UserModel.IsBookmarked = !UserModel.IsBookmarked;
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

        private async void OpenCVLinkCommandExecute(object obj)
        {
            try
            {
                Uri cvLink = new Uri(UserModel.Cv);
                Device.OpenUri(cvLink);
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Error opening CV.", "Close");
            }
        }

        private async void SelectTestimonialCommandExecute(object obj)
        {
            if (obj is TestimonialServerModel testimonialServerModel)
            {
                await NavigationService.NavigateToAsync<ViewTestimonialViewModel>(testimonialServerModel);
            }
        }

        private async void ViewAllTestimonialsCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<AllTestimonialsViewModel>(UserModel.Testimonials);
        }

        private async void ConnectCommandExecute(object obj)
        {
            var vm = new ConnectToPersonDialogViewModel();
            vm.UserModel = UserModel;
            await NavigationService.DisplayDialog(vm);
        }

        private async void RecommendCommandExecute(object obj)
        {
            await NavigationService.DisplayDialog(new NotImplementedDialogViewModel());
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            LoadUserInfo();
        }

        public override Task Init(object args)
        {
            if (args is string id)
            {
                userId = id;
            }

            return base.Init(args);
        }

        private void SetTabs()
        {
            Tabs = new ObservableCollection<TabIndicatorModel>();
            Tabs.Add(new TabIndicatorModel { Title = "Professional" });
            Tabs.Add(new TabIndicatorModel { Title = "Experience" });
            Tabs.Add(new TabIndicatorModel { Title = "Honors" });
        }

        private void TabSelectedCommandExecute(object obj)
        {
            var tabIndex = (int)obj;

            FirstTabActive = false;
            SecondTabActive = false;
            ThirdTabActive = false;

            switch (tabIndex)
            {
                case 0:
                    FirstTabActive = true;
                    break;
                case 1:
                    SecondTabActive = true;
                    break;
                case 2:
                    ThirdTabActive = true;
                    break;
                default:
                    FirstTabActive = true;
                    break;
            }
        }

        private async void LoadUserInfo()
        {
            if (IsLoaded)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            try
            {
                IsBusy = true;

                var result = await userService.GetUserById(userId);

                if (result.IsSuccess)
                {
                    UserModel = result;
                }

                IsLoaded = true;
            }
            catch (ServiceAuthenticationException e)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
            }
            catch (Exception e)
            {
                await NavigationService.DisplayAlert("Error", "Error loading user data. Check internet connection and try again.", "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

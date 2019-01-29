using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.ViewModels.Authentication;
using ImpressoApp.Services.AuthenticationService;
using ImpressoApp.Models;
using System.Threading.Tasks;
using ImpressoApp.Exceptions;
using ImpressoApp.Services.User;
using ImpressoApp.Services.RequestProvider;

namespace ImpressoApp.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUserService userService;
        private readonly IRequestProvider requestProvider;

        public bool EnableQuickLogin { get; set; }
        public UserInfoModel UserInfoModel { get; private set; }
        public string UserProfileImage { get; set; }

        public ICommand SignInToAnotherAccountCommand { get; private set; }
        public ICommand SignUpTpLogotypeCommand { get; private set; }
        public ICommand QuickLoginCommand { get; private set; }

        public WelcomeViewModel(INavigationService navigationService,
                                IRequestProvider requestProvider,
                                IAuthenticationService authenticationService,
                                IUserService userService) : base(navigationService)
        {
            this.requestProvider = requestProvider;
            this.authenticationService = authenticationService;
            this.userService = userService;

            SignInToAnotherAccountCommand = new Command(SignInToAnotherAccountCommandExecute);
            SignUpTpLogotypeCommand = new Command(SignUpTpLogotypeCommandExecute);
            QuickLoginCommand = new Command(QuickLoginCommandExecute);
        }

        private async void QuickLoginCommandExecute(object obj)
        {
            NavigationService.IsCompany = await authenticationService.GetIsCompany();
            await NavigationService.NavigateToAsync<MainViewModel>();
            NavigationService.ClearBackStack();
        }

        private void SignUpTpLogotypeCommandExecute(object obj)
        {
            NavigationService.NavigateToAsync<WalkthroughtViewModel>();
        }

        private void SignInToAnotherAccountCommandExecute(object obj)
        {
            NavigationService.NavigateToAsync<LoginViewModel>();
        }

        public async override void OnAppearing()
        {
            await LoadUserInfo();

            base.OnAppearing();
        }

        public async Task LoadUserInfo()
        {

            if (string.IsNullOrWhiteSpace(Settings.Token))
            {
                await Task.Delay(1);
                await NavigationService.NavigateToAsync<LoginViewModel>();
                return;
            }

            try
            {
                IsBusy = true;

                requestProvider.InitWithAuthorizationToken(Settings.Token);

                var result = await userService.GetUserNameWithPhoto();
                if (result.IsSuccess)
                {
                    Settings.UserName = result.UserName;
                    Settings.UserProfileImage = result.Photo;
                }

                UpdateUI();
            }
            catch (ServiceAuthenticationException ex)
            {
                await NavigationService.NavigateToAsync<LoginViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await NavigationService.DisplayAlert("Error", "Error loading. Check your internet connection and try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateUI()
        {
            EnableQuickLogin = !string.IsNullOrEmpty(Settings.Token);
            UserInfoModel = new UserInfoModel
            {
                UserName = Settings.UserName,
                UserProfileImage = Settings.UserProfileImage
            };
        }
    }
}

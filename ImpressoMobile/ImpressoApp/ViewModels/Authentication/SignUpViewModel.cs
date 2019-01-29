using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using ImpressoApp.Enums;
using Xamarin.Forms;
using ImpressoApp.Models;
using System.Collections.ObjectModel;
using ImpressoApp.Controls;
using System;
using Android.Appwidget;
using ImpressoApp.Services.Facebook;
using ImpressoApp.Services.AuthenticationService;
using ImpressoApp.Services.RequestProvider;

namespace ImpressoApp.ViewModels.Authentication
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly IFacebookService facebookService;
        private readonly IAuthenticationService authenticationService;
        private readonly IRequestProvider requestProvider;
        
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public UserType UserType { get; set; } = UserType.Person;

        public ICommand ContinueCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand FacebookLoginCommand { get; private set; }
        public ICommand TabChangedCommand { get; private set; }

        public ObservableCollection<TabIndicatorModel> Tabs { get; set; }

        public SignUpViewModel(INavigationService navigationService, 
                               IAuthenticationService authenticationService,
                               IFacebookService facebookService,
                               IRequestProvider requestProvider) : base(navigationService)
        {
            this.authenticationService = authenticationService;
            this.facebookService = facebookService;
            this.requestProvider = requestProvider;

            TabChangedCommand = new Command(TabSelectedCommandExecute);
            ContinueCommand = new Command(ContinueCommandExecute);
            LoginCommand = new Command(LoginCommandExecute);
            FacebookLoginCommand = new Command(FacebookLoginCommandExecute);

            SetupToggleTabs();
        }

        private async void FacebookLoginCommandExecute(object obj)
        {
            try
            {
                var loginResult = await facebookService.Login();

                switch (loginResult.LoginState)
                {
                    case FacebookLoginState.Success:
                        IsBusy = true;
                        LoginFacebookAsync(loginResult.Token);
                        Console.WriteLine("Facebook Login Success");
                        break;
                    case FacebookLoginState.Canceled:
                        Console.WriteLine("Facebook Login Cancelled");
                        break;
                    case FacebookLoginState.Failed:
                        await NavigationService.DisplayAlert("Error", "Error facebook login.", "Ok");
                        Console.WriteLine("Facebook Login Failed");
                        break;
                }
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Error login via Facebook. Try login using email instead.", "Ok");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoginFacebookAsync(string facebookToken)
        {
            try
            {
                IsBusy = true;

                var result = await authenticationService.LoginFacebookAsync(facebookToken);


                if (result.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(result.Jwt.AuthToken))
                    {
                        var token = result.Jwt.AuthToken;
                        requestProvider.InitWithAuthorizationToken(token);
                        Settings.Token = token;
                        Settings.UserName = result.Jwt.UserName;
                        Settings.UserID = result.Jwt.ID;

                        NavigationService.IsCompany = await authenticationService.GetIsCompany();
                    }

                    IsBusy = false;

                    await NavigationService.NavigateToAsync<MainViewModel>();
                    NavigationService.ClearBackStack();
                }
                else
                {
                    await NavigationService.DisplayAlert("Error", result.Message, "Ok");
                }
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Error sign up via Facebook. Try sign up using Email.", "Cancel");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void TabSelectedCommandExecute(object obj)
        {
            if(obj is int index)
            {
                this.UserType = (UserType)index + 1;
            }
        }

        private void SetupToggleTabs()
        {
            Tabs = new ObservableCollection<TabIndicatorModel>();
            Tabs.Add(new TabIndicatorModel { Title = "Person" });
            Tabs.Add(new TabIndicatorModel { Title = "Business" });
        }

        private async void LoginCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<LoginViewModel>();
        }

        private async void ContinueCommandExecute(object obj)
        {
            if(string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await NavigationService.DisplayAlert("Warning", "Email and Password should be filled.", "Cancel", "Ok");
                return;
            }

            if(!Password.Equals(ConfirmPassword))
            {
                await NavigationService.DisplayAlert("Error", "Passwords don't match.", "Ok");
                return;
            }

            var userModel = new UserInfoModel
            {
                Email = Email,
                Password = Password,
                UserType = UserType
            };

            await NavigationService.NavigateToAsync<SignUpSecondViewModel>(userModel);
        }

    }
}

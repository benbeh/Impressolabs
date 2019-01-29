using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.Services.AuthenticationService;
using ImpressoApp.Services.RequestProvider;
using ImpressoApp.Services.Facebook;
using ImpressoApp.Enums;
using ImpressoApp.Models;
using ImpressoApp.Models.Authentication;
using ImpressoApp.Services.User;

namespace ImpressoApp.ViewModels.Authentication
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IRequestProvider requestProvider;
        private readonly IFacebookService facebookService;
        private readonly IUserService userService;

        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand SignUpCommand { get; private set; }
        public ICommand LogInCommand { get; private set; }
        public ICommand FacebookLogInCommand { get; private set; }

        public LoginViewModel(INavigationService navigationService,
                              IAuthenticationService authenticationService,
                              IRequestProvider requestProvider,
                              IFacebookService facebookService,
                              IUserService userService) : base(navigationService)
        {
            this.authenticationService = authenticationService;
            this.requestProvider = requestProvider;
            this.facebookService = facebookService;
            this.userService = userService;

            Title = "Log in";            

            SignUpCommand = new Command(SignUpCommandExecute);
            LogInCommand = new Command(LogInCommandExecute);
            FacebookLogInCommand = new Command(FacebookLoginCommandExecute);
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
                        LoginAsync(loginResult.Token);
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
        }

        private void LogInCommandExecute(object obj)
        {
            LoginAsync();
        }

        private async void LoginAsync(string facebookToken = null)
        {
            try
            {
                IsBusy = true;

                LoginResponseModel result;

                if(!string.IsNullOrEmpty(facebookToken))
                {
                    result = await authenticationService.LoginFacebookAsync(facebookToken);
                }
                else
                {
                    result = await authenticationService.LoginAsync(Email, Password);
                }

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

                    var userInfo = await userService.GetUserNameWithPhoto();
                    if(userInfo.IsSuccess)
                    {
                        Settings.UserProfileImage = userInfo.Photo;
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
                await NavigationService.DisplayAlert("Error", ex.Message, "Cancel");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void SignUpCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<SignUpViewModel>();
        }
    }
}

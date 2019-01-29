using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.Services.AuthenticationService;
using System.Threading.Tasks;
using ImpressoApp.Models;
using ImpressoApp.Services.RequestProvider;
using ImpressoApp.Services;

namespace ImpressoApp.ViewModels.Authentication
{
    public class FinishSignUpViewModel : BaseViewModel
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IRequestProvider requestProvider;
        private readonly IPlatformService platformService;

        private UserInfoModel userModel;

        private bool _firstAppear = true;

        public ICommand GoToInboxCommand { get; private set; }


        public FinishSignUpViewModel(INavigationService navigationService,
                                     IRequestProvider requestProvider,
                                     IAuthenticationService authenticationService, IPlatformService platformService) : base(navigationService)
        {
            this.requestProvider = requestProvider;
            this.authenticationService = authenticationService;
            this.platformService = platformService;

            GoToInboxCommand = new Command(GoToInboxCommandExecute);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            if (!_firstAppear)
            {
                TryLogin(false);
            }

            _firstAppear = false;
        }

        private async void GoToInboxCommandExecute(object obj)
        {
            TryLogin(true);
        }

        private async void TryLogin(bool goToEmail)
        {
            try
            {
                if (userModel == null)
                {
                    return;
                }

                IsBusy = true;

                var result = await authenticationService.LoginAsync(userModel.Email, userModel.Password);
                if (result.IsSuccess)
                {
                    if (!string.IsNullOrWhiteSpace(result.Jwt.AuthToken))
                    {
                        Settings.ClearSettings();

                        Settings.Token = result.Jwt.AuthToken;
                        Settings.UserName = result.Jwt.UserName;
                        Settings.UserProfileImage = userModel.UserProfileImage;
                        Settings.UserID = result.Jwt.ID;
                        requestProvider.InitWithAuthorizationToken(result.Jwt.AuthToken);
                        NavigationService.IsCompany = await authenticationService.GetIsCompany();
                    }

                    IsBusy = false;

                    await NavigationService.NavigateToAsync<MainViewModel>();
                    NavigationService.ClearBackStack();
                }
                else
                {
                    if (!goToEmail || !platformService.GoToInbox())
                    {
                        await NavigationService.DisplayAlert("Error", "Please check your email and confirm registration", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }


        public override Task Init(object args)
        {
            userModel = args as UserInfoModel;

            return base.Init(args);
        }
    }
}

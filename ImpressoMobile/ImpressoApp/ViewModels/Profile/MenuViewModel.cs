using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Collections.ObjectModel;
using ImpressoApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.ViewModels.Authentication;
using ImpressoApp.Services.AuthenticationService;
using ImpressoApp.Services.Facebook;

namespace ImpressoApp.ViewModels.Profile
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly IFacebookService facebookService;

        public ObservableCollection<MenuItemModel> MenuItems { get; set; } = new ObservableCollection<MenuItemModel>();

        public string UserProfileImagePath { get; set; }
        public string UserName { get; set; }
        public string UserPosition { get; set; }

        public ICommand ChangeProfileCommand => new Command(NotImplementedCommandExecute);
        public ICommand JobOffersCommand => new Command(NotImplementedCommandExecute);
        public ICommand IntroductionCommand => new Command(NotImplementedCommandExecute);
        public ICommand SettingsCommand => new Command(NotImplementedCommandExecute);
        public ICommand PrivacyCommand => new Command(NotImplementedCommandExecute);
        public ICommand BuyTokensCommand => new Command(NotImplementedCommandExecute);
        public ICommand EventsCommand => new Command(NotImplementedCommandExecute);
        public ICommand LogoutCommand => new Command(LogoutCommandExecute); 
        public ICommand EditCommand => new Command(EditCommandExecute);

        public ICommand MenuItemTapCommand { get; private set; }

        public MenuViewModel(INavigationService navigationService, 
                             IFacebookService facebookService) : base(navigationService)
        {

            this.facebookService = facebookService;

            Title = "Menu";

            MenuItemTapCommand = new Command(MenuItemTapCommandExecute);

            InitUi();
        }

        private async void NotImplementedCommandExecute(object obj)
        {
            await NavigationService.DisplayDialog(new NotImplementedDialogViewModel());
        }

        private void LogoutCommandExecute(object obj)
        {
            try
            {
                facebookService.Logout();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Settings.Token = string.Empty;

            NavigationService.ClearBackStack();
            NavigationService.SetMainViewModel<LoginViewModel>();
        }

        private async void EditCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<EditProfileViewModel>();
        }

        private void MenuItemTapCommandExecute(object obj)
        {
            if (obj is MenuItemModel itemModel)
            {
                itemModel.TapCommand?.Execute(null);
            }
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            UserProfileImagePath = Settings.UserProfileImage;
            UserName = Settings.UserName;
            UserPosition = Settings.UserPosition;
        }

        private void InitUi()
        {
            var changeProfileItem = new MenuItemModel
            {
                Title = "Change Profile",
                TapCommand = ChangeProfileCommand
            };

            var jobOffersItem = new MenuItemModel
            {
                Title = "Job offers",
                TapCommand = JobOffersCommand
            };

            var introductionItem = new MenuItemModel
            {
                Title = "Introduction",
                TapCommand = IntroductionCommand
            };

            var settingsItem = new MenuItemModel
            {
                Title = "Settings",
                TapCommand = SettingsCommand
            };

            var privacyItem = new MenuItemModel
            {
                Title = "Privacy",
                TapCommand = PrivacyCommand
            };

            var buyTokensItem = new MenuItemModel
            {
                Title = "Buy Tokens",
                TapCommand = BuyTokensCommand
            };

            var eventsItem = new MenuItemModel
            {
                Title = "Events",
                TapCommand = EventsCommand
            };

            var logoutItem = new MenuItemModel
            {
                Title = "Logout",
                TapCommand = LogoutCommand
            };


            MenuItems.Add(changeProfileItem);
            MenuItems.Add(jobOffersItem);
            MenuItems.Add(introductionItem);
            MenuItems.Add(settingsItem);
            MenuItems.Add(privacyItem);
            MenuItems.Add(buyTokensItem);
            MenuItems.Add(eventsItem);
            MenuItems.Add(logoutItem);
        }
    }
}

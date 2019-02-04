using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Collections.ObjectModel;
using ImpressoApp.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImpressoApp.ViewModels.Profile
{
    public class MenuDialogViewModel : DialogViewModel
    {
        private readonly INavigationService navigationService;

        public ObservableCollection<MenuItemModel> MenuItems { get; set; } = new ObservableCollection<MenuItemModel>();

        public ICommand ChangeProfileCommand { get; set; }
        public ICommand JobOffersCommand { get; set; }
        public ICommand IntroductionCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand PrivacyCommand { get; set; }
        public ICommand BuyTokensCommand { get; set; }
        public ICommand EventsCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public ICommand MenuItemTapCommand { get; private set; }


        public MenuDialogViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            MenuItemTapCommand = new Command(MenuItemTapCommandExecute);

            InitUi();
        }

        private void MenuItemTapCommandExecute(object obj)
        {
            if(obj is MenuItemModel itemModel)
            {
                itemModel.TapCommand?.Execute(null);
            }
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

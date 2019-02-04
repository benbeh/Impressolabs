using System;
using System.Windows.Input;
using Autofac;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using ImpressoApp.Models.User;
using Xamarin.Forms;
using ImpressoApp.Services.Connect;
using ImpressoApp.Exceptions;
using System.Collections.ObjectModel;
using ImpressoApp.Models.Connect;
using System.Runtime.CompilerServices;

namespace ImpressoApp.ViewModels
{
    public class ConnectToPersonDialogViewModel : DialogViewModel
    {


        private INavigationService navigationService;
        private IConnectService connectService;

        public ICommand CancelCommand => new Command(CancelCommandExecute);

        public ICommand ConnectToPersonCommand => new Command(ConnectToPersonCommandExecute);
        public ICommand SelectRequestOptionCommand => new Command(SelectRequestOptionCommandExecite);

        public ConnectRequestOptionModel CurrentSelectedOption;

        public UserModel UserModel { get; set; }

        public ObservableCollection<ConnectRequestOptionModel> RequestOptions { get; set; }

        public bool IsEntryFocused { get; set; }

        public int HeightForKeyboard { get; set; }

        public ConnectToPersonDialogViewModel()
        {
            navigationService = (Application.Current as App)?.Container.Resolve<INavigationService>();
            connectService = (Application.Current as App)?.Container.Resolve<IConnectService>();

            RequestOptions = new ObservableCollection<ConnectRequestOptionModel>
            {
                new ConnectRequestOptionModel{ Name = "Professional date (Meet up for coffee, lunch or dinner)"},
                new ConnectRequestOptionModel{ Name = "Request assistance"},
                new ConnectRequestOptionModel{ Name = "Job advertisement"},
                new ConnectRequestOptionModel{ Name = "To ask you advice on:"}
            };
        }

        private void SelectRequestOptionCommandExecite(object obj)
        {
            if (obj is ConnectRequestOptionModel model)
            {
                model.IsSelected = true;
                if (CurrentSelectedOption == null)
                {
                    CurrentSelectedOption = model;
                }
                else
                {
                    CurrentSelectedOption.IsSelected = false;
                    CurrentSelectedOption = model;
                }
            }
        }

        private void CancelCommandExecute(object obj)
        {
            navigationService?.HideDialog();
        }

        private async void ConnectToPersonCommandExecute(object obj)
        {
            try
            {
                var result = await connectService.ConnectToPersonAsync(UserModel.Id);

                if (result.IsSuccess)
                {
                    UserModel.IsConnected = true;
                    navigationService?.HideDialog();
                }
                else
                {
                    await navigationService.DisplayAlert("", result.Message, "Ok");
                }
            }
            catch (ServiceAuthenticationException e)
            {
                await navigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
            }
            catch (Exception ex)
            {
                await navigationService.DisplayAlert("Error", "Error sending connection request. Probably you have already connected with this person.", "Close");
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsEntryFocused))
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    HeightForKeyboard = IsEntryFocused ? 250 : 0;
                }
            }
        }
    }
}

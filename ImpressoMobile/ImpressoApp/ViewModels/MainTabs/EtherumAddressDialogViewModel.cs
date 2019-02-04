using System;
using BaseMvvmToolkit.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.Services.User;
using BaseMvvmToolkit.Services;
using ImpressoApp.Exceptions;
using Autofac;

namespace ImpressoApp.ViewModels.MainTabs
{
    public class EtherumAddressDialogViewModel : DialogViewModel
    {
        private readonly IUserService userService;
        protected readonly INavigationService navigationService;

        public string Address { get; set; }

        public ICommand SendCommand { get; set; }
        public ICommand DismissCommand { get; set; }
        public ICommand DoNotShowAgainCommand { get; set; }

        public EtherumAddressDialogViewModel()
        {
            this.navigationService = (Application.Current as App)?.Container.Resolve<INavigationService>();
            this.userService = (Application.Current as App)?.Container.Resolve<IUserService>();

            SendCommand = new Command(SendCommandExecute);
            DismissCommand = new Command(CancelCommandExecute);
            DoNotShowAgainCommand = new Command(DoNotShowAgainExecute);
        }

        public async void DoNotShowAgainExecute()
        {
            try
            {
                IsBusy = true;

                Settings.DoNotShowAgainEthereumDialogEmail = Settings.UserName;

                await navigationService.HideDialog();
            }
            catch (ServiceAuthenticationException)
            {
                await navigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", string.Empty, "Close");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await navigationService.DisplayAlert("Error", "Unknown issue. Please try again", string.Empty, "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void SendCommandExecute()
        {
            if (string.IsNullOrEmpty(Address))
            {
                await navigationService.DisplayAlert("Error", "Address is empty", "Close");
            }

            try
            {
                IsBusy = true;

                await userService.SetEthereumAddressAsync(Address);

                navigationService.HideDialog();
            }
            catch (ServiceAuthenticationException)
            {
                await navigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", string.Empty, "Close");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await navigationService.DisplayAlert("Error", "Unknown issue. Please try again", string.Empty, "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void CancelCommandExecute()
        {
            navigationService.HideDialog();
        }
    }
}

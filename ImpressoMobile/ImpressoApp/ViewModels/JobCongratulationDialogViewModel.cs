using System;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using ImpressoApp.Pages.Feeds;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImpressoApp.ViewModels
{
    public class CongratulationDialogViewModel : DialogViewModel
    {
        private readonly INavigationService navigationService;

        public ICommand ContinueCommand => new Command(async () => await navigationService.HideDialog());

        public CongratulationDialogViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}

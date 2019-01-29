using System;
using BaseMvvmToolkit.ViewModels;
using Xamarin.Forms;
using BaseMvvmToolkit.Services;
using ImpressoApp.ViewModels.MainTabs;
namespace ImpressoApp.ViewModels
{
    public class MainViewModel : TabbedViewModel<FeedsTabViewModel, SearchTabViewModel, MessagesTabViewModel, ProfileTabViewModel>
    {
        public MainViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}

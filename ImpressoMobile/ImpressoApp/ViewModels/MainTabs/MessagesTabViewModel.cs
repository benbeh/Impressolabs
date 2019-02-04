using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
namespace ImpressoApp.ViewModels.MainTabs
{
    public class MessagesTabViewModel : BaseViewModel
    {
        public MessagesTabViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Messages";
            Icon = "messageOutline";
        }
    }
}

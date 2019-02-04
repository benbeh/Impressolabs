using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BaseMvvmToolkit.ViewModels
{
    public interface IBaseViewModel : INotifyPropertyChanged
    {
        Task Init(object args);

        void OnAppearing();

        void OnDisappearing();

        bool OnBackButtonPressed();

        bool IsBusy { get; set; }

        bool IsCustomDialogShowed { get; set; }

        string Title { get; set; }

        string Icon { get; set; }

        ObservableCollection<ToolbarItem> ToolbarItems { get; set; }

        Type PageType { get; }

        Type DrawerMenuViewModelType { get; }
    }
}

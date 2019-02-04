using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BaseMvvmToolkit.Services;
using Xamarin.Forms;

namespace BaseMvvmToolkit.ViewModels
{
    public class BaseViewModel : AbstractNpcObject, IBaseViewModel
    {
        private object _initArgs;
        private bool _started;

        protected readonly INavigationService NavigationService;

        protected BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            ToolbarItems = new ObservableCollection<ToolbarItem>();

            Title = string.Empty;
        }

        public bool IsCompany => NavigationService.IsCompany;

        public bool IsVisible { get; private set; }

        public ITabbedViewModel ParentViewModel { get; set; }

        public bool IsBusy { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public virtual Type DrawerMenuViewModelType => null;

        public virtual Type PageType => null;

        public ObservableCollection<ToolbarItem> ToolbarItems { get; set; }

        public bool IsCustomDialogShowed { get; set; }

        public virtual async Task Init(object args)
        {
            _initArgs = args;
        }

        public virtual void OnStart()
        {
            _started = true;
        }

        public virtual void OnAppearing()
        {
            IsVisible = true;

            if (!_started)
            {
                _started = true;
                OnStart();
            }
        }

        public virtual void OnDisappearing()
        {
            IsVisible = false;
        }

        public virtual bool OnBackButtonPressed()
        {
            return false;
        }
    }
}

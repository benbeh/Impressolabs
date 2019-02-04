using System;
using System.Collections.Generic;
using BaseMvvmToolkit.Services;

namespace BaseMvvmToolkit.ViewModels
{
    public abstract class TabbedViewModel<TViewModel1, TViewModel2> : BaseViewModel, ITabbedViewModel
          where TViewModel1 : IBaseViewModel
          where TViewModel2 : IBaseViewModel
    {
        protected TabbedViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ChildViewModels = new Dictionary<string, BaseViewModel>();
        }

        public IDictionary<string, BaseViewModel> ChildViewModels { get; set; }

        public event EventHandler<ViewModelSelectionArgs> SelectedPageChange;

        public void SelectTab(Type viewModelType)
        {
            SelectedPageChange?.Invoke(this, new ViewModelSelectionArgs { SelectedViewModelType = viewModelType });
        }

        public virtual void OnSelectedTabChanged(BaseViewModel selectedViewModel)
        {
        }
    }

    public abstract class TabbedViewModel<TViewModel1, TViewModel2, TViewModel3> : BaseViewModel, ITabbedViewModel
        where TViewModel1 : IBaseViewModel
        where TViewModel2 : IBaseViewModel
        where TViewModel3 : IBaseViewModel
    {
        public TabbedViewModel(INavigationService navigationService)
          : base(navigationService)
        {
            ChildViewModels = new Dictionary<string, BaseViewModel>();
        }

        public IDictionary<string, BaseViewModel> ChildViewModels { get; set; }

        public event EventHandler<ViewModelSelectionArgs> SelectedPageChange;

        public void SelectTab(Type viewModelType)
        {
            SelectedPageChange?.Invoke(this, new ViewModelSelectionArgs { SelectedViewModelType = viewModelType });
        }

        public virtual void OnSelectedTabChanged(BaseViewModel selectedViewModel)
        {
        }
    }

    public abstract class TabbedViewModel<TViewModel1, TViewModel2, TViewModel3, TViewModel4> : BaseViewModel, ITabbedViewModel
        where TViewModel1 : IBaseViewModel
        where TViewModel2 : IBaseViewModel
        where TViewModel3 : IBaseViewModel
        where TViewModel4 : IBaseViewModel
    {
        public TabbedViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ChildViewModels = new Dictionary<string, BaseViewModel>();
        }

        public IDictionary<string, BaseViewModel> ChildViewModels { get; set; }

        public event EventHandler<ViewModelSelectionArgs> SelectedPageChange;

        public void SelectTab(Type viewModelType)
        {
            SelectedPageChange?.Invoke(this, new ViewModelSelectionArgs { SelectedViewModelType = viewModelType });
        }

        public virtual void OnSelectedTabChanged(BaseViewModel selectedViewModel)
        {
        }
    }

    public abstract class TabbedViewModel<TViewModel1, TViewModel2, TViewModel3, TViewModel4, TViewModel5> : BaseViewModel, ITabbedViewModel
        where TViewModel1 : IBaseViewModel
        where TViewModel2 : IBaseViewModel
        where TViewModel3 : IBaseViewModel
        where TViewModel4 : IBaseViewModel
        where TViewModel5 : IBaseViewModel
    {
        public TabbedViewModel(INavigationService navigationService)
          : base(navigationService)
        {
            ChildViewModels = new Dictionary<string, BaseViewModel>();
        }

        public IDictionary<string, BaseViewModel> ChildViewModels { get; set; }

        public event EventHandler<ViewModelSelectionArgs> SelectedPageChange;

        public void SelectTab(Type viewModelType)
        {
            SelectedPageChange?.Invoke(this, new ViewModelSelectionArgs { SelectedViewModelType = viewModelType });
        }

        public virtual void OnSelectedTabChanged(BaseViewModel selectedViewModel)
        {
        }
    }

    public abstract class TabbedViewModel<TViewModel1, TViewModel2, TViewModel3, TViewModel4, TViewModel5, TViewModel6> : BaseViewModel, ITabbedViewModel
        where TViewModel1 : IBaseViewModel
        where TViewModel2 : IBaseViewModel
        where TViewModel3 : IBaseViewModel
        where TViewModel4 : IBaseViewModel
        where TViewModel5 : IBaseViewModel
        where TViewModel6 : IBaseViewModel
    {
        public TabbedViewModel(INavigationService navigationService)
          : base(navigationService)
        {
            ChildViewModels = new Dictionary<string, BaseViewModel>();
        }

        public IDictionary<string, BaseViewModel> ChildViewModels { get; set; }

        public event EventHandler<ViewModelSelectionArgs> SelectedPageChange;

        public void SelectTab(Type viewModelType)
        {
            SelectedPageChange?.Invoke(this, new ViewModelSelectionArgs { SelectedViewModelType = viewModelType });
        }

        public virtual void OnSelectedTabChanged(BaseViewModel selectedViewModel)
        {
        }
    }
}

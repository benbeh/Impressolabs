using System;
using System.Collections.Generic;

namespace BaseMvvmToolkit.ViewModels
{
    public interface ITabbedViewModel : IBaseViewModel
    {
        event EventHandler<ViewModelSelectionArgs> SelectedPageChange;

        IDictionary<string, BaseViewModel> ChildViewModels { get; set; }

        void SelectTab(Type viewModelType);

        void OnSelectedTabChanged(BaseViewModel selectedViewModel);
    }
}

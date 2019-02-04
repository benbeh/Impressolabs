using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseMvvmToolkit.ViewModels;
using Xamarin.Forms;

namespace BaseMvvmToolkit.Services
{
    public interface INavigationService
    {
        bool IsCompany { get; set; }

        Task DisplayAlert(string title, string message, string cancel);

        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);

        Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons);

        void SetMainViewModel<T>(object args = null) where T : IBaseViewModel;

        Task NavigateToAsync<T>(object args = null) where T : IBaseViewModel;

        Task NavigateToAsync(Type baseViewModelPage, object args = null);

        Task NavigateToModalAsync<T>(object args = null) where T : IBaseViewModel;

        void NavigateToMenuItem<T>(object args = null) where T : IBaseViewModel;

        Task PopAsync(object args = null);

        Task PopModalAsync(object args = null);

        Task PopToRootAsync();

        Page ResolvePageFor<T>(object args = null) where T : IBaseViewModel;

        void RemoveFromNavigationStack<T>(bool removeFirstOccurenceOnly = true) where T : IBaseViewModel;

        void ClearBackStack();

        IReadOnlyList<IBaseViewModel> GetNavigationStack();

        bool IsRootPage { get; }

        IBaseViewModel CurrentViewModel { get; }

        Page CurrentPage { get; }

        void OpenDrawerMenu();

        void CloseDrawerMenu();

        void ToggleDrawerMenu();

        Task DisplayDialog<T>(object args = null) where T : IDialogViewModel;

        Task DisplayDialog(IDialogViewModel dialogViewModel, object args = null);

        Task HideDialog();

        SlideCustomDialog ResolveDialogFor<T>(object args = null) where T : IDialogViewModel;

        void OnActivated();
    }
}

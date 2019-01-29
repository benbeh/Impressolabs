using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BaseMvvmToolkit.Extensions;
using BaseMvvmToolkit.PageLocators;
using BaseMvvmToolkit.Pages;
using BaseMvvmToolkit.ViewModels;
using Xamarin.Forms;
using BaseMvvmToolkit.DialogLocator;

namespace BaseMvvmToolkit.Services
{
    public class NavigationService : INavigationService
    {
        protected INavigation Navigation { get; private set; }
        protected IPageLocator PageLocator { get; }
        protected ICustomDialogLocator CustomDialogLocator { get; }
        protected MasterDetailPage MasterDetailPage { get; private set; }

        public NavigationService(IPageLocator pageLocator, ICustomDialogLocator customDialogLocator)
        {
            PageLocator = pageLocator;
            CustomDialogLocator = customDialogLocator;
        }

        public bool IsCompany { get; set; }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await CurrentPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await CurrentPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons)
        {
            return await CurrentPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public void SetMainViewModel<T>(object args = null) where T : IBaseViewModel
        {
            var page = ResolvePageFor<T>(args);

            if (page == null)
            {
                throw new Exception("Resolve page for " + typeof(T).Name + " returned null!");
            }

            if (page is MasterDetailPage masterDetailPage)
            {

                masterDetailPage.Detail = new BaseNavigationPage(masterDetailPage.Detail);
                Navigation = masterDetailPage.Detail.Navigation;
                Application.Current.MainPage = masterDetailPage;
                MasterDetailPage = masterDetailPage;
            }
            else
            {
                var navigationPage = new BaseNavigationPage(page, false);
                Navigation = navigationPage.Navigation;
                Application.Current.MainPage = navigationPage;
            }
        }

        public void NavigateToMenuItem<T>(object args = null) where T : IBaseViewModel
        {
            var page = ResolvePageFor<T>(args);

            if (page == null)
            {
                throw new Exception("Resolve page for " + typeof(T).Name + " returned null!");
            }

            if (MasterDetailPage == null)
            {
                return;
            }

            if (page is MasterDetailPage masterDetailPage)
            {
                MasterDetailPage.Detail = new BaseNavigationPage(masterDetailPage.Detail);
                Navigation = masterDetailPage.Detail.Navigation;
            }

            else
            {
                throw new Exception(typeof(T).Name + "page type should be BasePage");
            }
        }

        public async Task NavigateToAsync(Type baseViewModelPage, object args = null)
        {
            if (!baseViewModelPage.IsAssignableTo<IBaseViewModel>())
                return;

            var page = ResolvePageFor(baseViewModelPage, args);

            if (page == null)
            {
                var msg = args?.GetType() + " " + args;
                Debug.WriteLine(msg);
                return;
            }

            await Navigation.PushAsync(page);
        }

        public async Task NavigateToAsync<T>(object args = null) where T : IBaseViewModel
        {
            var page = ResolvePageFor<T>(args);

            if (page == null)
            {
                var msg = args?.GetType() + " " + args;
                Debug.WriteLine(msg);
                return;
            }

            await Navigation.PushAsync(page);
        }

        public async Task NavigateToAsync<T>(T type, object args = null) where T : IBaseViewModel
        {
            await NavigateToAsync<T>(args);
        }

        public Task NavigateToModalAsync<T>(object args = null) where T : IBaseViewModel
        {
            var page = ResolvePageFor<T>(args);

            return Navigation.PushModalAsync(page);
        }

        public Task PopAsync(object args = null)
        {
            if (args != null)
            {
                var navigationStack = GetNavigationStack();
                var viewModelToPop = navigationStack.Count > 1 ? navigationStack.SecondLast() : navigationStack.Last();
                viewModelToPop?.Init(args);
            }

            return Navigation.PopAsync();
        }

        public Task PopModalAsync(object args = null)
        {
            if (args != null)
            {
                var modalStack = GetModalStack();
                var viewModelToPop = modalStack.Count > 1 ? modalStack.SecondLast() : modalStack.Last();
                viewModelToPop?.Init(args);
            }

            return Navigation.PopModalAsync();
        }

        public Task PopToRootAsync()
        {
            return Navigation.PopToRootAsync();
        }

        public Page ResolvePageFor<T>(object args = null) where T : IBaseViewModel
        {
            var page = PageLocator.ResolvePageAndViewModel(typeof(T), args);

            return page;
        }
        public Page ResolvePageFor(Type pageType, object args = null)
        {
            var page = PageLocator.ResolvePageAndViewModel(pageType, args);
            return page;
        }

        public void RemoveFromNavigationStack<T>(bool removeFirstOccurenceOnly = true) where T : IBaseViewModel
        {
            var pageType = typeof(T).IsAssignableTo<ITabbedViewModel>()
                ? typeof(BaseTabbedPage)
                : PageLocator.ResolvePageType(typeof(T));

            var navigationStack = Navigation.NavigationStack.Reverse();

            foreach (var page in navigationStack)
            {
                if (page.GetType() == pageType)
                {
                    Navigation.RemovePage(page);

                    if (removeFirstOccurenceOnly)
                    {
                        break;
                    }
                }
            }
        }

        public void ClearBackStack()
        {
            var stack = Navigation.NavigationStack.ToList();
            var currentPage = CurrentPage;

            foreach (var page in stack)
            {
                if (page != currentPage && page != Navigation.NavigationStack.LastOrDefault())
                {
                    Navigation.RemovePage(page);
                }
            }
        }

        public IReadOnlyList<IBaseViewModel> GetNavigationStack()
        {
            return Navigation.NavigationStack.Select(page => page.BindingContext as IBaseViewModel).ToList();
        }

        public IReadOnlyList<IBaseViewModel> GetModalStack()
        {
            return Navigation.ModalStack.Select(page => page.BindingContext as IBaseViewModel).ToList();
        }

        public bool IsRootPage => Navigation.NavigationStack.Count == 1;

        public IBaseViewModel CurrentViewModel => CurrentPage?.BindingContext as IBaseViewModel;

        public Page CurrentPage
        {
            get
            {
                var page = Navigation?.NavigationStack?.LastOrDefault();
                if (page is BaseTabbedPage tabbedPage)
                {
                    return tabbedPage.CurrentPage;
                }
                return page;
            }
        }

        public void OpenDrawerMenu()
        {
            PresentDrawerMenu(true);
        }

        public void CloseDrawerMenu()
        {
            PresentDrawerMenu(false);
        }

        public void ToggleDrawerMenu()
        {
            if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
            {
                masterDetailPage.IsPresented = !masterDetailPage.IsPresented;
            }
        }

        private void PresentDrawerMenu(bool isPresented)
        {
            if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
            {
                masterDetailPage.IsPresented = isPresented;
            }
        }

        public async Task DisplayDialog<T>(object arg = null) where T : IDialogViewModel
        {
            var dialog = ResolveDialogFor<T>(arg);

            if (CurrentPage is MenuContainerPage page)
            {
                await page.HideMenu();
                page.SlideMenu = dialog;
                await page.ShowMenu();
            }
        }

        public SlideCustomDialog ResolveDialogFor<T>(object args = null) where T : IDialogViewModel
        {
            var dialog = CustomDialogLocator.ResolveDialogAndViewModel(typeof(T), args);

            return dialog;
        }

        public async Task DisplayDialog(IDialogViewModel dialogViewModel, object args = null)
        {
            var dialog = CustomDialogLocator.ResolveDialog(dialogViewModel);

            if (CurrentPage is MenuContainerPage page)
            {
                await page.HideMenu();
                page.SlideMenu = dialog;
                await page.ShowMenu();
            }
        }

        public async Task HideDialog()
        {
            if (CurrentPage is MenuContainerPage page)
            {
                await page.HideMenu();
            }
        }

        public void OnActivated()
        {
            (CurrentPage?.BindingContext as BaseViewModel)?.OnAppearing();
        }
    }
}

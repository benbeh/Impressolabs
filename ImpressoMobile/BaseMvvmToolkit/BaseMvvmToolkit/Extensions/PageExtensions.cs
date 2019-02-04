using BaseMvvmToolkit.Pages;
using BaseMvvmToolkit.ViewModels;
using Xamarin.Forms;

namespace BaseMvvmToolkit.Extensions
{
    public static class PageExtensions
    {
        public static void BindViewModel(this IBasePage page, IBaseViewModel viewModel)
        {
            page.BindingContext = viewModel;
            page.SetBinding<IBaseViewModel>(Page.IsBusyProperty, "IsBusy");
            page.SetBinding<IBaseViewModel>(Page.TitleProperty, "Title");
            page.SetBinding<IBaseViewModel>(Page.IconProperty, "Icon");
            if (page is MenuContainerPage)
                page.SetBinding<IBaseViewModel>(MenuContainerPage.IsCustomDialogShowedProperty, "IsCustomDialogShowed");

            page.Appearing += (sender, args) => viewModel.OnAppearing();
            page.Disappearing += (sender, args) => viewModel.OnDisappearing();
        }
    }
}

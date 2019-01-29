using System;
using BaseMvvmToolkit.ViewModels;
using Xamarin.Forms;

namespace BaseMvvmToolkit.PageLocators
{
    public interface IPageLocator
    {
        Page ResolvePageAndViewModel(Type viewModelType, object args = null);

        Page ResolvePage(IBaseViewModel viewModel);

        Type ResolvePageType(Type viewmodel);
    }
}

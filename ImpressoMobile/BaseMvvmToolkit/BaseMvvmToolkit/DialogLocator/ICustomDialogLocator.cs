using System;
using BaseMvvmToolkit.ViewModels;
namespace BaseMvvmToolkit.DialogLocator
{
    public interface ICustomDialogLocator
    {
        SlideCustomDialog ResolveDialogAndViewModel(Type dialogViewModelType, object args = null);

        SlideCustomDialog ResolveDialog(IDialogViewModel dialogViewModel);

        Type ResolveDialogType(Type dialogViewModel);
    }
}

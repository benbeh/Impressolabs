using System;
using BaseMvvmToolkit.ViewModels;

namespace BaseMvvmToolkit.DialogLocator
{
    public class CustomDialogLocator : ICustomDialogLocator
    {
        public CustomDialogLocator()
        {
        }

        protected virtual SlideCustomDialog CreateDialog(Type dialogType)
        {
            return Activator.CreateInstance(dialogType) as SlideCustomDialog;
        }

        protected virtual IDialogViewModel CreateDialogViewModel(Type dialogViewModelType)
        {
            return Activator.CreateInstance(dialogViewModelType) as IDialogViewModel;
        }

        protected virtual Type FindDialogTypeForViewModel(Type viewModelType)
        {
            var dialogTypeName = viewModelType
                .AssemblyQualifiedName
                .Replace("DialogViewModel", "Dialog")
                .Replace("ViewModels", "Pages");

            var pageType = Type.GetType(dialogTypeName);

            if (pageType == null)
            {
                throw new ArgumentException("Can't find a dialog of type '" + dialogTypeName + "' for ViewModel '" +
                                            viewModelType.Name + "'");
            }

            return pageType;
        }

        public Type ResolveDialogType(Type dialogViewModel)
        {
            return FindDialogTypeForViewModel(dialogViewModel);
        }

        public SlideCustomDialog ResolveDialog(IDialogViewModel dialogViewModel)
        {
            var dialogType = dialogViewModel.DialogType ?? ResolveDialogType(dialogViewModel.GetType());
            var dialog = CreateDialog(dialogType);

            if (!(dialog is SlideCustomDialog))
            {
                throw new ArgumentException("Page for '" + dialogViewModel.GetType().Name +
                                                            "' should be of type 'BasePage' instead of '" +
                                            dialogType.Name + "'");
            }

            dialog.BindingContext = dialogViewModel;

            return dialog as SlideCustomDialog;
        }

        public SlideCustomDialog ResolveDialogAndViewModel(Type dialogViewModelType, object args = null)
        {
            SlideCustomDialog dialog;

            var dialogViewModel = CreateDialogViewModel(dialogViewModelType);

            dialog = ResolveDialog(dialogViewModel);

            dialogViewModel.Init(args);

            return dialog;

        }


    }
}

using System;
using System.Collections.ObjectModel;
using BaseMvvmToolkit.ViewModels;
using ImpressoApp.Services.Project;
using System.Linq;
using Xamarin.Forms;
using Autofac;
using System.Windows.Input;
using BaseMvvmToolkit.Services;
using ImpressoApp.Models.Job;
using ImpressoApp.Services.User;
using System.Collections.Generic;
using ImpressoApp.Models.Project;

namespace ImpressoApp.ViewModels
{
    public class NotImplementedDialogViewModel : DialogViewModel
    {
        public ICommand CancelCommand { get; set; }

        public NotImplementedDialogViewModel()
        {
            CancelCommand = new Command(Cancel);
        }

        public void Cancel()
        {
            var navigationService = (Application.Current as App)?.Container.Resolve<INavigationService>();
            navigationService.HideDialog();
        }
    }
}

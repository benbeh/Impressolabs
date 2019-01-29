using System;
using FFImageLoading.Transformations;
using Xamarin.Forms;
using BaseMvvmToolkit.ViewModels;
using System.Windows.Input;

namespace ImpressoApp.ViewModels.Profile
{
    public class DeleteTagDialogViewModel : DialogViewModel
    {
        public ICommand YesCommand { get; set; }
        public ICommand NoCommand { get; set; }

    }
}

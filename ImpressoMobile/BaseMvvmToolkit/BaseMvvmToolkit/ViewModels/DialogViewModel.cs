using System;
namespace BaseMvvmToolkit.ViewModels
{
    public class DialogViewModel : AbstractNpcObject, IDialogViewModel
    {
        private object args;

        public DialogViewModel()
        {
        }

        public Type DialogType { get; set; }

        public virtual void Init(object arg)
        {
            this.args = arg;
        }

        public bool IsBusy { get; set; }
    }
}

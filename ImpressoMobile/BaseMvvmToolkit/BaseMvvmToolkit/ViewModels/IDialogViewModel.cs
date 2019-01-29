using System;
namespace BaseMvvmToolkit.ViewModels
{
    public interface IDialogViewModel
    {
        void Init(object arg);

        Type DialogType { get; }
    }
}

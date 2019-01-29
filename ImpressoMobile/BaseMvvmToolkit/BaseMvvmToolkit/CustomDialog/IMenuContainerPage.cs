using System;
using System.Threading.Tasks;

namespace BaseMvvmToolkit
{
    public interface IMenuContainerPage
    {
        SlideCustomDialog SlideMenu { get; set; }

        Action ShowMenuAction { get; set; }

        Action HideMenuAction { get; set; }

        Task ShowMenu();

        Task HideMenu();

        bool IsCustomDialogShowed { get; set; }

        void OnAnimationCompleted();
    }
}


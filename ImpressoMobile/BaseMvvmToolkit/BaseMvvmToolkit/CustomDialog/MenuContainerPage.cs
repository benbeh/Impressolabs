using System;

using Xamarin.Forms;
using System.Collections.Generic;
using BaseMvvmToolkit.Pages;
using System.Threading.Tasks;

namespace BaseMvvmToolkit
{
    public class MenuContainerPage : BasePage, IMenuContainerPage
    {
        public static BindableProperty IsCustomDialogShowedProperty = BindableProperty.Create(nameof(IsCustomDialogShowed), typeof(bool), typeof(BasePage), false, BindingMode.TwoWay, propertyChanged: CustomDialogShowChanged);

        private TaskCompletionSource<object> _animationCompletionSource;

        static void CustomDialogShowChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if ((bool)newValue)
            {
                ((MenuContainerPage)bindable).ShowMenu();
            }
            else
            {
                ((MenuContainerPage)bindable).HideMenu();
            }
        }


        public bool IsCustomDialogShowed
        {
            get { return (bool)GetValue(IsCustomDialogShowedProperty); }
            set { SetValue(IsCustomDialogShowedProperty, value); }
        }


        public MenuContainerPage()
        {

        }

        SlideCustomDialog slideMenu;
        public SlideCustomDialog SlideMenu
        {
            get
            {
                return slideMenu;
            }
            set
            {
                if (slideMenu != null)
                    slideMenu.Parent = null;
                slideMenu = value;
                if (slideMenu != null)
                    slideMenu.Parent = this;
            }
        }

        public Action ShowMenuAction { get; set; }

        public Action HideMenuAction { get; set; }

        public void OnAnimationCompleted()
        {
            _animationCompletionSource.TrySetResult(null);
        }

        public Task ShowMenu()
        {
            if (IsCustomDialogShowed)
            {
                return Task.FromResult<object>(null);
            }
            IsCustomDialogShowed = true;
            _animationCompletionSource = new TaskCompletionSource<object>();
            ShowMenuAction();
            return _animationCompletionSource.Task;
        }

        public Task HideMenu()
        {
            if (!IsCustomDialogShowed)
            {
                return Task.FromResult<object>(null);
            }

            IsCustomDialogShowed = false;
            _animationCompletionSource = new TaskCompletionSource<object>();
            HideMenuAction();
            return _animationCompletionSource.Task;
        }
    }
}



using System;
using BaseMvvmToolkit.ViewModels;
using BottomBar.XamarinForms;
using Xamarin.Forms;

namespace BaseMvvmToolkit.Pages
{
    public class BaseTabbedPage : BottomBarPage, IBasePage
    {
        public static BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(BaseTabbedPage), Color.FromHex("#7E20C8"));

        public void SetBinding<TSource>(BindableProperty targetProperty, string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null, string stringFormat = null)
        {
            this.SetBinding(targetProperty, path, mode,
                converter, stringFormat);
        }

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public event PageClosedEventHandler PageClosing;

        public void OnPageClosing()
        {
            PageClosing?.Invoke(this, new EventArgs());
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            FixedMode = true;

            if (BindingContext is ITabbedViewModel viewModel && viewModel.ToolbarItems != null && viewModel.ToolbarItems.Count > 0)
            {
                foreach (var toolBarItem in viewModel.ToolbarItems)
                {
                    if (!(ToolbarItems.Contains(toolBarItem)))
                    {
                        ToolbarItems.Add(toolBarItem);
                    }
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            OnPageClosing();

            var viewModel = BindingContext as IBaseViewModel;
            var result = viewModel?.OnBackButtonPressed() ?? base.OnBackButtonPressed();
            return result;
        }
    }
}

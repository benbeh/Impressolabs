using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using BaseMvvmToolkit.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BaseMvvmToolkit.Pages
{
    public delegate void PageClosedEventHandler(object sender, EventArgs e);

    public class BasePage : ContentPage, IBasePage
    {
        public static readonly BindableProperty BackButtonIconProperty = BindableProperty.Create(nameof(BackButtonIcon), typeof(string), typeof(BasePage), "toolbar_arrow");
        public static BindableProperty BackButtonCommandProperty = BindableProperty.Create(nameof(BackButtonCommand), typeof(ICommand), typeof(BasePage));

        public string BackButtonIcon
        {
            get { return (string)GetValue(BackButtonIconProperty); }
            set { SetValue(BackButtonIconProperty, value); }
        }

        public ICommand BackButtonCommand
        {
            get { return (ICommand)GetValue(BackButtonCommandProperty); }
            set { SetValue(BackButtonCommandProperty, value); }
        }

        public BasePage()
        {
            //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        public void SetBinding<TSource>(BindableProperty targetProperty,
            string path, BindingMode mode = BindingMode.Default,
            IValueConverter converter = null, string stringFormat = null)
        {
            this.SetBinding(targetProperty, path, mode,
                converter, stringFormat);
        }

        public event PageClosedEventHandler PageClosing;

        public void OnPageClosing()
        {
            PageClosing?.Invoke(this, new EventArgs());
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var viewModel = BindingContext as IBaseViewModel;

            if (viewModel?.ToolbarItems == null)
            {
                return;
            }

            viewModel.ToolbarItems.CollectionChanged += ViewModel_ToolbarItems_CollectionChanged;

            foreach (var toolBarItem in viewModel.ToolbarItems)
            {
                if (ToolbarItems.All(x => x.Text != toolBarItem.Text))
                {
                    ToolbarItems.Add(toolBarItem);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            var viewModel = BindingContext as IBaseViewModel;
            var result = viewModel?.OnBackButtonPressed() ?? base.OnBackButtonPressed();
            return result;
        }

        private void ViewModel_ToolbarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ToolbarItems.Clear();

            if (!(sender is ObservableCollection<ToolbarItem> vmToolbar))
            {
                return;
            }

            foreach (var item in vmToolbar)
            {
                if (ToolbarItems.All(x => x.Text != item.Text))
                {
                    ToolbarItems.Add(item);
                }
            }
        }
    }
}

using System;
using Xamarin.Forms;
namespace ImpressoApp.Controls
{
    public class TabContainer : ContentView
    {
        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TabContainer), "Tab");
        public static BindableProperty TabSelectedIconProperty = BindableProperty.Create(nameof(TabSelectedIcon), typeof(FileImageSource), typeof(TabContainer), default(FileImageSource));
        public static BindableProperty TabIconProperty = BindableProperty.Create(nameof(TabIcon), typeof(FileImageSource), typeof(TabContainer), default(FileImageSource));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public FileImageSource TabSelectedIcon
        {
            get => (FileImageSource)GetValue(TabSelectedIconProperty);
            set => SetValue(TabSelectedIconProperty, value);
        }

        public FileImageSource TabIcon
        {
            get => (FileImageSource)GetValue(TabIconProperty);
            set => SetValue(TabIconProperty, value);
        }

        public TabContainer()
        {
        }
    }
}

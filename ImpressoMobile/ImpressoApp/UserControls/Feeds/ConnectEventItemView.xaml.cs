using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImpressoApp.UserControls.Feeds
{
    public partial class ConnectEventItemView
    {
        public static BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(ConnectEventItemView));
        public static BindableProperty InterestedTapCommandProperty = BindableProperty.Create(nameof(InterestedTapCommand), typeof(ICommand), typeof(ConnectEventItemView));
        public static BindableProperty ShareTapCommandProperty = BindableProperty.Create(nameof(ShareTapCommand), typeof(ICommand), typeof(ConnectEventItemView));
        public static BindableProperty OptionTapCommandProperty = BindableProperty.Create(nameof(OptionTapCommand), typeof(ICommand), typeof(ConnectEventItemView));

        public ICommand TapCommand
        {
            get { return (ICommand)GetValue(TapCommandProperty); }
            set { SetValue(TapCommandProperty, value); }
        }

        public ICommand InterestedTapCommand
        {
            get { return (ICommand)GetValue(InterestedTapCommandProperty); }
            set { SetValue(InterestedTapCommandProperty, value); }
        }

        public ICommand ShareTapCommand
        {
            get { return (ICommand)GetValue(ShareTapCommandProperty); }
            set { SetValue(ShareTapCommandProperty, value); }
        }

        public ICommand OptionTapCommand
        {
            get { return (ICommand)GetValue(OptionTapCommandProperty); }
            set { SetValue(OptionTapCommandProperty, value); }
        }

        public ConnectEventItemView()
        {
            InitializeComponent();
        }
    }
}

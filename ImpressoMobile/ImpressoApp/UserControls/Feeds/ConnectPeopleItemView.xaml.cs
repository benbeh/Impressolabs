using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImpressoApp.UserControls.Feeds
{
    public partial class ConnectPeopleItemView
    {
        public static BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(ConnectPeopleItemView));
        public static BindableProperty RecommendTapCommandProperty = BindableProperty.Create(nameof(RecommendTapCommand), typeof(ICommand), typeof(ConnectPeopleItemView));
        public static BindableProperty ConnectTapCommandProperty = BindableProperty.Create(nameof(ConnectTapCommand), typeof(ICommand), typeof(ConnectPeopleItemView));
        public static BindableProperty BookmarkTapCommandProperty = BindableProperty.Create(nameof(BookmarkTapCommand), typeof(ICommand), typeof(ConnectPeopleItemView));

        public Command TapCommand
        {
            get { return (Command)GetValue(TapCommandProperty); }
            set { SetValue(TapCommandProperty, value); }
        }

        public Command RecommendTapCommand
        {
            get { return (Command)GetValue(RecommendTapCommandProperty); }
            set { SetValue(RecommendTapCommandProperty, value); }
        }

        public Command ConnectTapCommand
        {
            get { return (Command)GetValue(ConnectTapCommandProperty); }
            set { SetValue(ConnectTapCommandProperty, value); }
        }

        public Command BookmarkTapCommand
        {
            get { return (Command)GetValue(BookmarkTapCommandProperty); }
            set { SetValue(BookmarkTapCommandProperty, value); }
        }

        public ConnectPeopleItemView()
        {
            InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ImpressoApp.Controls;
using System.Windows.Input;

namespace ImpressoApp.UserControls.Cell
{
    public partial class SmallIconInfoCell : CustomViewCell
    {

        public SmallIconInfoCell()
        {
            InitializeComponent();
        }

        public static BindableProperty GestureRecognizerProperty = BindableProperty.Create(nameof(GestureRecognizer), typeof(IGestureRecognizer), typeof(SmallIconInfoCell), propertyChanged: (bindable, b, newValue) =>
        {
            if (bindable is SmallIconInfoCell cell)
            {
                cell.View.GestureRecognizers.Clear();
                if (newValue != null)
                {
                    cell.View.GestureRecognizers.Add((IGestureRecognizer)newValue);
                }
            }

        });

        public IGestureRecognizer GestureRecognizer
        {
            get { return (IGestureRecognizer)GetValue(GestureRecognizerProperty); }
            set
            {
                SetValue(GestureRecognizerProperty, value);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public static BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(SmallIconInfoCell));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static BindableProperty MainTextProperty = BindableProperty.Create(nameof(MainText), typeof(string), typeof(SmallIconInfoCell), defaultValue: string.Empty);

        public string MainText
        {
            get { return (string)GetValue(MainTextProperty); }
            set { SetValue(MainTextProperty, value); }
        }

        public static BindableProperty SecondaryTextProperty = BindableProperty.Create(nameof(SecondaryText), typeof(string), typeof(SmallIconInfoCell), defaultValue: string.Empty);

        public string SecondaryText
        {
            get { return (string)GetValue(SecondaryTextProperty); }
            set { SetValue(SecondaryTextProperty, value); }
        }

        public static BindableProperty DetailTextProperty = BindableProperty.Create(nameof(DetailText), typeof(string), typeof(SmallIconInfoCell), defaultValue: string.Empty);

        public string DetailText
        {
            get { return (string)GetValue(DetailTextProperty); }
            set { SetValue(DetailTextProperty, value); }
        }

        public static BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(SmallIconInfoCell));

        public ICommand TapCommand
        {
            get { return (ICommand)GetValue(TapCommandProperty); }
            set { SetValue(TapCommandProperty, value); }
        }
    }
}

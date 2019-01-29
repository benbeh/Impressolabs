using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ImpressoApp.UserControls
{
    public partial class BusyIndicator : Grid
    {
        public static readonly BindableProperty TextInfoProperty =
            BindableProperty.Create(nameof(TextInfo), typeof(string), typeof(BusyIndicator), string.Empty, BindingMode.TwoWay, propertyChanged: TextInfoPropertyChanged);

        public static readonly BindableProperty IsActiveProperty =
            BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(BusyIndicator), false, BindingMode.TwoWay, propertyChanged: IsActivePropertyChanged);

        public string TextInfo
        {
            get
            {
                return (string)GetValue(TextInfoProperty);
            }
            set
            {
                if (value != null)
                    SetValue(TextInfoProperty, value);
            }
        }

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(TextInfoProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        public BusyIndicator()
        {
            InitializeComponent();
        }

        public void UpdateText(string indicatorDescription)
        {
            DescriptionLabel.Text = indicatorDescription;
        }

        public void UpdateActiveState(bool isActive)
        {
            IsVisible = isActive;
            Indicator.IsRunning = isActive;
        }

        private static void IsActivePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var sender = bindable as BusyIndicator;
            if (sender != null)
            {
                sender.UpdateActiveState((bool)newValue);
            }
        }

        private static void TextInfoPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var sender = bindable as BusyIndicator;
            if (sender != null)
            {
                sender.UpdateText((string)newValue);
            }
        }
    }
}

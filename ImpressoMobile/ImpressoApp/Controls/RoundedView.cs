using System;
using System.Windows.Input;
using Xamarin.Forms;
namespace ImpressoApp.Controls
{
    public class RoundedView : ContentView
    {
        public static BindableProperty BorderRadiusProperty = BindableProperty.Create(nameof(BorderRadius), typeof(int), typeof(RoundedView), 0);
        public static BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(RoundedView), 0);
        public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RoundedView), Color.Default);
        public static BindableProperty StartColorProperty = BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(RoundedView), Color.Transparent);
        public static BindableProperty EndColorProperty = BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(RoundedView), Color.Transparent);
        public static BindableProperty UseGradientProperty = BindableProperty.Create(nameof(UseGradient), typeof(bool), typeof(RoundedView), false);

        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public int BorderRadius
        {
            get { return (int)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        public bool UseGradient
        {
            get { return (bool)GetValue(UseGradientProperty); }
            set { SetValue(UseGradientProperty, value); }
        }
    }
}

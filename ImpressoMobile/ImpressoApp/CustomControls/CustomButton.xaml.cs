using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Windows.Input;
using ImpressoApp.Controls;
using Android.Text;

namespace ImpressoApp.CustomControls
{
    public partial class CustomButton : ContentView
    {
        public static readonly BindableProperty BorderRadiusProperty =
            BindableProperty.Create(nameof(BorderRadius), typeof(int), typeof(CustomButton), 0, BindingMode.TwoWay);

        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(CustomButton), Color.Transparent);

        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(CustomButton), Color.Transparent);

        public static readonly BindableProperty BackColorProperty =
            BindableProperty.Create(nameof(BackColor), typeof(Color), typeof(CustomButton), Color.Transparent);

        public static readonly BindableProperty UseGradientProperty =
            BindableProperty.Create(nameof(UseGradient), typeof(bool), typeof(CustomButton), false);

        public static readonly BindableProperty ButtonTextProperty =
            BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(CustomButton), "");

        public static readonly BindableProperty CommandProperty = 
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomButton), null );

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomButton), null);
         
        
        public int BorderRadius
        {
            get => (int)GetValue(BorderRadiusProperty);
            set => SetValue(BorderRadiusProperty, value);
        }

        public Color StartColor 
        {
            get => (Color)GetValue(StartColorProperty);
            set => SetValue(StartColorProperty, value);
        }

        public Color EndColor
        {
            get => (Color)GetValue(EndColorProperty);
            set => SetValue(EndColorProperty, value);
        }

        public Color BackColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public bool UseGradient
        {
            get => (bool)GetValue(UseGradientProperty);
            set => SetValue(UseGradientProperty, value);
        }

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public CustomButton()
        {
            InitializeComponent();

            Layout.SetBinding(RoundedView.BorderRadiusProperty, new Binding(nameof(BorderRadius), source: this));
            Layout.SetBinding(RoundedView.StartColorProperty, new Binding(nameof(StartColor), source: this));
            Layout.SetBinding(RoundedView.EndColorProperty, new Binding(nameof(EndColor), source: this));
            Layout.SetBinding(RoundedView.UseGradientProperty, new Binding(nameof(UseGradient), source: this));
            TextLabel.SetBinding(Label.TextProperty, new Binding(nameof(ButtonText), source: this));


            var tapGesture = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (Command != null)
                    {
                        if (Command.CanExecute(CommandParameter))
                        {
                            Command.Execute(CommandParameter);
                        }
                    }
                })
            };

            this.GestureRecognizers.Add(tapGesture);
        }

    }
}

using System;
using Xamarin.Forms;

namespace ImpressoApp.Controls
{
    public class CustomEntry : Entry
    {
        public static BindableProperty DisplaySuggestionsProperty = BindableProperty.Create(nameof(DisplaySuggestions), typeof(bool), typeof(CustomEntry), true);
        public static BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(CustomEntry), 0);
        public static BindableProperty BorderRadiusProperty = BindableProperty.Create(nameof(BorderRadius), typeof(int), typeof(CustomEntry), 0);
        public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEntry), Color.Default);
        public static BindableProperty EntryBackgroundColorProperty = BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(CustomEntry), Color.Default);

        public bool DisplaySuggestions
        {
            get { return (bool)GetValue(DisplaySuggestionsProperty); }
            set { SetValue(DisplaySuggestionsProperty, value); }
        }

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

        public Color EntryBackgroundColor
        {
            get { return (Color)GetValue(EntryBackgroundColorProperty); }
            set { SetValue(EntryBackgroundColorProperty, value); }
        }
    }
}

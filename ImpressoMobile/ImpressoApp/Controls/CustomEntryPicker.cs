using System;
using Xamarin.Forms;
namespace ImpressoApp.Controls
{
    public class CustomEntryPicker : Picker
    {
        public static BindableProperty DisplaySuggestionsProperty = BindableProperty.Create(nameof(DisplaySuggestions), typeof(bool), typeof(CustomEntryPicker), true);
        public static BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(CustomEntryPicker), 0);
        public static BindableProperty BorderRadiusProperty = BindableProperty.Create(nameof(BorderRadius), typeof(int), typeof(CustomEntryPicker), 0);
        public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEntryPicker), Color.Default);
        public static BindableProperty EntryBackgroundColorProperty = BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(CustomEntryPicker), Color.Default);
        public static BindableProperty HorizontalTextAligmentProperty = BindableProperty.Create(nameof(HorizontalTextAligment), typeof(TextAlignment), typeof(CustomEntryPicker), TextAlignment.Start);

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

        public TextAlignment HorizontalTextAligment 
        {
            get => (TextAlignment)GetValue(HorizontalTextAligmentProperty);
            set => SetValue(HorizontalTextAligmentProperty, value);
        }
    }
}

using ImpressoApp.Controls;
using ImpressoApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Graphics.Drawables;
using ImpressoApp.Droid.Utils;

[assembly: ExportRenderer(typeof(CustomEntryPicker), typeof(CustomEntryPickerRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class CustomEntryPickerRenderer : PickerRenderer
    {
        public CustomEntryPickerRenderer(Context context) : base(context){}

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var entry = Element as CustomEntryPicker;

                var gradient = new GradientDrawable();
                var padding = (int)AndroidUtils.ConvertDpToPixel(Context, 10);

                if (entry != null)
                {
                    gradient.SetStroke(entry.BorderThickness, entry.BorderColor.ToAndroid());
                    gradient.SetCornerRadius(AndroidUtils.ConvertDpToPixel(Context, entry.BorderRadius));
                    gradient.SetColor(entry.EntryBackgroundColor.ToAndroid());
                }

                if (!entry.DisplaySuggestions)
                {
                    Control.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
                }

                Control.Background = gradient;
                Control.SetPadding(padding, padding, padding, padding);

                switch(entry.HorizontalTextAligment)
                {
                    case Xamarin.Forms.TextAlignment.Start:
                        Control.TextAlignment = Android.Views.TextAlignment.TextStart;
                        break;
                    case Xamarin.Forms.TextAlignment.Center:
                        Control.TextAlignment = Android.Views.TextAlignment.Center;
                        Control.Gravity = Android.Views.GravityFlags.CenterHorizontal;
                        break;
                    case Xamarin.Forms.TextAlignment.End:
                        Control.TextAlignment = Android.Views.TextAlignment.TextEnd;
                        break;
                    default:
                        Control.TextAlignment = Android.Views.TextAlignment.TextStart;
                        break;
                }
            }
        }
    }
}

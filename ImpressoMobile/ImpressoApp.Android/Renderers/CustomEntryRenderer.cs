using ImpressoApp.Controls;
using ImpressoApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Graphics.Drawables;
using ImpressoApp.Droid.Utils;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context){}

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var entry = Element as CustomEntry;

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
                Control.SetPadding(padding, 0, padding, 0);
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;

                switch (entry.HorizontalTextAlignment)
                {
                    case Xamarin.Forms.TextAlignment.Start:
                        Control.TextAlignment = Android.Views.TextAlignment.TextStart;
                        break;
                    case Xamarin.Forms.TextAlignment.Center:
                        Control.TextAlignment = Android.Views.TextAlignment.Center;
                        Control.Gravity = Android.Views.GravityFlags.CenterHorizontal | Android.Views.GravityFlags.CenterVertical;
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

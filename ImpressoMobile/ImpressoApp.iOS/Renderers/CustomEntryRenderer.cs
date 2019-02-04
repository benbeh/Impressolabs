using CoreGraphics;
using ImpressoApp.Controls;
using ImpressoApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var entry = Element as CustomEntry;

                if (entry != null)
                {
                    NativeView.Layer.CornerRadius = entry.BorderRadius;
                    NativeView.Layer.BorderWidth = entry.BorderThickness;
                    NativeView.Layer.BorderColor = entry.BorderColor.ToCGColor();
                    NativeView.Layer.BackgroundColor = entry.EntryBackgroundColor.ToCGColor();
                }

                if (entry != null && !entry.DisplaySuggestions)
                {
                    Control.AutocorrectionType = UITextAutocorrectionType.No;
                }

                Control.LeftView = new UIView(new CGRect(0, 0, 15, Control.Frame.Height));
                Control.RightView = new UIView(new CGRect(0, 0, 15, Control.Frame.Height));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.RightViewMode = UITextFieldViewMode.Always;
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}

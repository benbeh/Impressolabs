using System;
using CoreGraphics;
using ImpressoApp.Controls;
using ImpressoApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntryPicker), typeof(CustomEntryPickerRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class CustomEntryPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var entry = Element as CustomEntryPicker;

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

                if(entry != null)
                {
                    switch(entry.HorizontalTextAligment)
                    {
                        case TextAlignment.Center:
                            Control.TextAlignment = UITextAlignment.Center;
                            break;
                        case TextAlignment.Start:
                            Control.TextAlignment = UITextAlignment.Left;
                            break;
                        case TextAlignment.End:
                            Control.TextAlignment = UITextAlignment.Right;
                            break;
                        default:
                            Control.TextAlignment = UITextAlignment.Natural;
                            break;
                    }
                }
            }
        }
    }
}

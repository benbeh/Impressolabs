using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ImpressoApp.Controls;
using ImpressoApp.iOS.Renderers;
using System.ComponentModel;
using UIKit;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}

using System;
using Xamarin.Forms.Platform.iOS;
using ImpressoApp.Controls;
using UIKit;
using Xamarin.Forms;
using ImpressoApp.iOS.Renderers;
using CoreGraphics;
using CoreAnimation;
using System.Linq;

[assembly: ExportRenderer(typeof(RoundedView), typeof(RoundedViewRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class RoundedViewRenderer : ViewRenderer<RoundedView, UIView>
    {
        private RoundedView roundedView;


        protected override void OnElementChanged(ElementChangedEventArgs<RoundedView> e)
        {
            base.OnElementChanged(e);

            roundedView = Element as RoundedView;
            if (roundedView != null)
            {
                NativeView.Layer.CornerRadius = roundedView.BorderRadius;
                NativeView.Layer.BorderWidth = roundedView.BorderThickness;
                NativeView.Layer.BorderColor = roundedView.BorderColor.ToCGColor();
                NativeView.Layer.BackgroundColor = roundedView.BackgroundColor.ToCGColor();
            }
        }       
    }
}

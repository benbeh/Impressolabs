using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using ImpressoApp.Controls;
using ImpressoApp.iOS.Renderers;
using CoreGraphics;
using CoreAnimation;

[assembly: ExportRenderer(typeof(GradientFrame), typeof(GradientFrameRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class GradientFrameRenderer : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            GradientFrame stack = (GradientFrame)Element;

            CGColor startColor = stack.StartColor.ToCGColor();
            CGColor endColor = stack.EndColor.ToCGColor();

            var gradientLayer = new CAGradientLayer
            {
                StartPoint = new CGPoint(0, 0.5),
                EndPoint = new CGPoint(1, 0.5)
            };

            gradientLayer.Frame = rect;
            gradientLayer.Colors = new CGColor[] { startColor, endColor };

            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}

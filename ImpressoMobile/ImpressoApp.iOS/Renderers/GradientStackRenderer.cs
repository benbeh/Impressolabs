using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using ImpressoApp.Controls;
using ImpressoApp.iOS.Renderers;
using CoreGraphics;
using CoreAnimation;

[assembly: ExportRenderer(typeof(GradientStack), typeof(GradientStackRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class GradientStackRenderer : VisualElementRenderer<StackLayout>
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            GradientStack stack = (GradientStack)Element;

            CGColor startColor = stack.StartColor.ToCGColor();
            CGColor endColor = stack.EndColor.ToCGColor();
            CAGradientLayer gradientLayer;

            if(stack.Orientation == StackOrientation.Horizontal)
            {
                gradientLayer = new CAGradientLayer
                {
                    StartPoint = new CGPoint(0, 0.5),
                    EndPoint = new CGPoint(1, 0.5)
                };
            }
            else
            {
                gradientLayer = new CAGradientLayer();
            }

            gradientLayer.Frame = rect;
            gradientLayer.Colors = new CGColor[] { startColor, endColor };

            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }  
    }
}

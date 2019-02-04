using System;
using System.ComponentModel;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ImpressoApp.Controls;
using ImpressoApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientButton), typeof(GradientButtonRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class GradientButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            UpdatePadding();

            var button = Element as GradientButton;

            if (e.OldElement == null)
            {
                var gradient = new CAGradientLayer()
                {
                    StartPoint = new CGPoint(0, 0.5),
                    EndPoint = new CGPoint(1, 0.5)
                };
                gradient.Locations = new NSNumber[] { .0f, 1f };
                gradient.CornerRadius = Element.CornerRadius;
                gradient.NeedsDisplayOnBoundsChange = true;
                gradient.MasksToBounds = true;

                CGColor startColor = button?.StartColor.ToCGColor();
                CGColor endColor = button?.EndColor.ToCGColor();
                gradient.Colors = new CGColor[] { startColor, endColor };

                var layer = Control?.Layer.Sublayers.FirstOrDefault();
                Control?.Layer.InsertSublayerBelow(gradient, layer);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(GradientButton.Padding))
            {
                UpdatePadding();
            }
        }

        private void UpdatePadding()
        {
            var element = this.Element as GradientButton;
            if (element != null)
            {
                this.Control.ContentEdgeInsets = new UIEdgeInsets(

                    (int)element.Padding.Top,
                    (int)element.Padding.Left,
                    (int)element.Padding.Bottom,
                    (int)element.Padding.Right
                );
            }
        }

        public override CGRect Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                if (value.Width > 0 && value.Height > 0)
                {
                    foreach (var layer in Control?.Layer.Sublayers.Where(layer => layer is CAGradientLayer))
                    {
                        layer.Frame = new CGRect(0, 0, value.Width, value.Height);
                    }
                }
                base.Frame = value;
            }
        }
    }
}

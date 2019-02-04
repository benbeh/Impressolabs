using System;
using ImpressoApp.Controls;
using ImpressoApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(FitImageButton), typeof(FitImageButtonRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class FitImageButtonRenderer : ButtonRenderer
    {
        private FitImageButton FitImageButton => Element as FitImageButton;

        public FitImageButtonRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var fileName = FitImageButton?.Image?.File;
            if (fileName == null)
            {
                return;
            }
            var btn = Control as UIButton;
            btn.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
        }
    }
}

using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Newtonsoft.Json.Serialization;

namespace ImpressoApp.iOS
{
    public class FormsElementWrapper : UIView
    {
        public IVisualElementRenderer Renderer { get; private set; }

        public FormsElementWrapper(Xamarin.Forms.View content, Element parent)
        {
            content.Parent = parent;

            Renderer = content != null ? Platform.CreateRenderer(content) : null;
            Platform.SetRenderer(content, Renderer);

            var cachedBindingContext = content.BindingContext;
            content.BindingContext = cachedBindingContext;
            if (Renderer == null)
                return;

            AddSubview(Renderer.NativeView);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Renderer.Element.Layout(new Rectangle(0.0, 0.0, Frame.Width, Frame.Height));
        }
    }
}

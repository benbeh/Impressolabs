using System;
using BaseMvvmToolkit.Pages;
using ImpressoApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BaseTabbedPage), typeof(BaseTabbedPageRenderer))]
namespace ImpressoApp.iOS.Renderers
{
    public class BaseTabbedPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement is BaseTabbedPage tabbedPage)
            {
                TabBar.TintColor = tabbedPage.TintColor.ToUIColor();
            }
        }
    }
}

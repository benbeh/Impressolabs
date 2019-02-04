using System;
using Xamarin.Forms;
using ImpressoApp.Droid.Renderers;
using Android.Content;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(PaddingButtonRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class PaddingButtonRenderer : ButtonRenderer
    {
        public PaddingButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            Control?.SetPadding(Control.PaddingLeft, 0, Control.PaddingRight, 0);
            Control.SetSingleLine();
        }
    }
}

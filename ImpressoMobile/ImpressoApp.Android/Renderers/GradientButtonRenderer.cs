using System;
using ImpressoApp.Controls;
using ImpressoApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Graphics.Drawables;
using ImpressoApp.Droid.Utils;
using Android.Util;
using System.Linq;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(GradientButton), typeof(GradientButtonRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class GradientButtonRenderer : ButtonRenderer
    {
        public GradientButtonRenderer(Context context) : base(context){}

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            UpdatePadding();

            //var button = Element as GradientButton;

            //if (button != null)
            //{
            //    button.SizeChanged += (s, args) =>
            //    {
            //        var colors = new int[]
            //        {
            //            button.StartColor.ToAndroid(),
            //            button.EndColor.ToAndroid()
            //        };

            //        var normal = new GradientDrawable(GradientDrawable.Orientation.LeftRight, colors);

            //        normal.SetCornerRadius(AndroidUtils.ConvertDpToPixel(Context, button.CornerRadius));
            //        normal.SetStroke((int)button.BorderWidth, button.BorderColor.ToAndroid());

            //        var pressed = new GradientDrawable();

            //        pressed.SetColor(button.StartColor.ToAndroid());
            //        pressed.SetCornerRadius(AndroidUtils.ConvertDpToPixel(Context, button.CornerRadius));
            //        pressed.SetStroke((int)button.BorderWidth, button.BorderColor.ToAndroid());

            //        var sld = new StateListDrawable();
            //        sld.AddState(new int[] { Android.Resource.Attribute.StatePressed }, pressed);
            //        sld.AddState(new int[] { }, normal);

            //        Control.SetBackgroundDrawable(sld);
            //    };
            //}
        }

        private void UpdatePadding()
        {
            var element = this.Element as GradientButton;
            if (element != null)
            {
                this.Control.SetPadding(
                    (int)element.Padding.Left,
                    (int)element.Padding.Top,
                    (int)element.Padding.Right,
                    (int)element.Padding.Bottom
                );
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
    }
}

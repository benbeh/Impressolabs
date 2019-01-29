using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using ImpressoApp.Controls;
using ImpressoApp.Droid.Renderers;
using Xamarin.Forms;
using ImpressoApp.Droid.Utils;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(RoundedView), typeof(RoundedViewRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class RoundedViewRenderer : ViewRenderer<RoundedView, Android.Views.View>
    {
        private RoundedView roundedView;
        private GradientDrawable gradient;

        public RoundedViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<RoundedView> e)
        {
            base.OnElementChanged(e);

            roundedView = Element as RoundedView;

            if (roundedView != null)
            {
                if(roundedView.UseGradient)
                {
                    var colors = new int[]
                    {
                        roundedView.StartColor.ToAndroid(),
                        roundedView.EndColor.ToAndroid()
                    };

                    gradient = new GradientDrawable(GradientDrawable.Orientation.LeftRight, colors);
                }
                else
                {
                    gradient = new GradientDrawable();
                    gradient.SetColor(roundedView.BackgroundColor.ToAndroid());
                }

                gradient.SetStroke(roundedView.BorderThickness, roundedView.BorderColor.ToAndroid());
                gradient.SetCornerRadius(AndroidUtils.ConvertDpToPixel(Context, roundedView.BorderRadius));

                Background = gradient;
            }
        }
    }
}

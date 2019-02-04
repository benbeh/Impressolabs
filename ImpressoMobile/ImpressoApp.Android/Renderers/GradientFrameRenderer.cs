using System;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;
using Android.Graphics;
using ImpressoApp.Controls;
using ImpressoApp.Droid.Renderers;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(GradientFrame), typeof(GradientFrameRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class GradientFrameRenderer : FrameRenderer
    {
        //private Color StartColor { get; set; }
        //private Color EndColor { get; set; }

        public GradientFrameRenderer(Context context) : base(context){}

        //protected override void DispatchDraw(Canvas canvas)
        //{
        //    var gradient = new LinearGradient(
        //    0, 0, Width, 0, StartColor.ToAndroid(), EndColor.ToAndroid(), Shader.TileMode.Mirror);

        //    var colors = new int[]
        //    { 
        //        StartColor.ToAndroid(), 
        //        EndColor.ToAndroid() 
        //    };

        //    //GradientDrawable gradient = new GradientDrawable(GradientDrawable.Orientation.LeftRight, colors);

        //    //canvas.dra
        //    var paint = new Paint { Dither = true };
        //    paint.SetShader(gradient);
        //    canvas.DrawPaint(paint);



        //    base.DispatchDraw(canvas);
        //}

        //protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.OldElement != null || Element == null)
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        var frame = e.NewElement as GradientFrame;
        //        StartColor = frame.StartColor;
        //        EndColor = frame.EndColor;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(@"Error: ", ex.Message);
        //    }
        //}
    }
}

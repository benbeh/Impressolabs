using System;
using Android.Content;
using ImpressoApp.Controls;
using ImpressoApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(GradientStack), typeof(GradientStackRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class GradientStackRenderer : VisualElementRenderer<StackLayout>
    {
        GradientStack stack;
        private Color StartColor { get; set; }  
        private Color EndColor { get; set; } 

        public GradientStackRenderer(Context context) : base(context){}

        protected override void DispatchDraw(Canvas canvas)
        {
            LinearGradient gradient;

            if(stack.Orientation == StackOrientation.Horizontal)
            {
                gradient = new LinearGradient(
                0, 0, Width, 0, StartColor.ToAndroid(), EndColor.ToAndroid(), Shader.TileMode.Mirror);
            }
            else
            {
                gradient = new LinearGradient(
                    0, 0, Height, 0, StartColor.ToAndroid(), EndColor.ToAndroid(), Shader.TileMode.Mirror);
            }


            var paint = new Paint { Dither = true };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);

            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null) 
            { 
                return; 
            } 

            try 
            { 
                stack = e.NewElement as GradientStack; 
                StartColor = stack.StartColor; 
                EndColor = stack.EndColor; 
            } 
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine(@"Error: ", ex.Message); 
            } 
        }
    }
}

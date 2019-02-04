using Android.Content;
using Android.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace ImpressoApp.Droid
{
    class FormsElementWrapper : ViewGroup
    {
        public IVisualElementRenderer Renderer { get; private set; }

        public FormsElementWrapper(Context context, Xamarin.Forms.View content, Element parent) : base(context)
        {
            content.Parent = parent;
            var renderer = content != null ? Platform.CreateRendererWithContext(content, Context) : null;
            var cachedBindingContext = content.BindingContext;
            content.BindingContext = cachedBindingContext;
            if (renderer == null)
                return;
            Renderer = renderer;
            AddView(renderer.View);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            Renderer.Element.Layout(new Rectangle(0.0, 0.0, ContextExtensions.FromPixels(Context, w), ContextExtensions.FromPixels(Context, h)));
            Renderer.UpdateLayout();
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            if (Renderer == null)
                return;
            Renderer.Element.Layout(new Rectangle(0.0, 0.0, ContextExtensions.FromPixels(Context, right - left), ContextExtensions.FromPixels(Context, bottom - top)));
            Renderer.UpdateLayout();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);
            int widthSize = MeasureSpec.GetSize(widthMeasureSpec);
            int heightSize = MeasureSpec.GetSize(heightMeasureSpec);
            int pxHeight = (int)ContextExtensions.ToPixels(Context, Renderer.Element.HeightRequest);
            int pxWidth = (int)ContextExtensions.ToPixels(Context, Renderer.Element.WidthRequest);
            var measuredWidth = widthMode != MeasureSpecMode.Exactly ? (widthMode != MeasureSpecMode.AtMost ? pxHeight : Math.Min(pxHeight, widthSize)) : widthSize;
            var measuredHeight = heightMode != MeasureSpecMode.Exactly ? (heightMode != MeasureSpecMode.AtMost ? pxWidth : Math.Min(pxWidth, heightSize)) : heightSize;
            SetMeasuredDimension(measuredWidth, measuredHeight);
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return true;
        }
    }

}
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Content;
using ImpressoApp.Controls;
using ImpressoApp.Droid;
using BaseMvvmToolkit;

[assembly: ExportRenderer(typeof(SlideCustomDialog), typeof(SlideMenuDroidRenderer))]
namespace ImpressoApp.Droid
{
    public class SlideMenuDroidRenderer : ViewRenderer<SlideCustomDialog, Android.Views.View>
    {
        IDialogDrawer _dragGesture;

        internal IDialogDrawer GragGesture { get { return _dragGesture; } }


        public SlideMenuDroidRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SlideCustomDialog> e)
        {
            base.OnElementChanged(e);
            InitDragGesture();
        }

        void InitDragGesture()
        {
            var menu = Element as SlideCustomDialog;

            if (menu == null)
                return;
            if (ScreenSizeHelper.ScreenHeight == 0 && ScreenSizeHelper.ScreenWidth == 0)
            {
                ScreenSizeHelper.ScreenWidth = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density;
                ScreenSizeHelper.ScreenHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;
            }
            _dragGesture = new VerticalDrawer(menu, this.Resources.DisplayMetrics.Density);
            _dragGesture.RequestLayout = (l, t, r, b, desity) =>
            {
                this.SetX((float)l);
                this.SetY((float)t);
            };
        }


    }
}


using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Content;
using ImpressoApp.Controls;
using Android.Animation;
using ImpressoApp.Droid;
using BaseMvvmToolkit;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(MenuContainerPage), typeof(MenuContainerPageDroidRenderer))]
namespace ImpressoApp.Droid
{
    public class MenuContainerPageDroidRenderer : PageRenderer
    {
        public MenuContainerPageDroidRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            _basePage = e.NewElement as IMenuContainerPage;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "SlideMenu")
            {
                AddMenu();
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (formsWrapper != null)
            {
                formsWrapper.Renderer.UpdateLayout();
                ViewGroup.BringChildToFront(formsWrapper);
            }
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            if (_basePage == null)
                return;

            var metrics = Resources.DisplayMetrics;
            ScreenSizeHelper.ScreenWidth = w / metrics.Density;
            ScreenSizeHelper.ScreenHeight = h / metrics.Density;

            var menu = _basePage.SlideMenu;
            if (menu != null)
            {
                if (_dialogDrawer != null)
                {
                    _dialogDrawer.UpdateLayoutSize(menu);

                    var rect = _dialogDrawer.GetHidePosition();
                    menu.Layout(new Xamarin.Forms.Rectangle(
                        rect.left / metrics.Density,
                        rect.top / metrics.Density,
                        (rect.right - rect.left) / metrics.Density,
                        (rect.bottom - rect.top) / metrics.Density));
                    if (formsWrapper != null)
                        formsWrapper.Renderer.UpdateLayout();
                    _dialogDrawer.LayoutHideStatus();
                    return;
                }
            }

            if (_backgroundOverlay != null)
                _backgroundOverlay.Layout(
                    0,
                    0,
                    (int)(ScreenSizeHelper.ScreenWidth * metrics.Density),
                    (int)(ScreenSizeHelper.ScreenHeight * metrics.Density));
        }

        IMenuContainerPage _basePage;
        IDialogDrawer _dialogDrawer;
        FormsElementWrapper formsWrapper;
        global::Android.Widget.LinearLayout _backgroundOverlay;


        void AddMenu()
        {
            if (_basePage == null)
                return;
            var menu = _basePage.SlideMenu;
            if (menu == null)
                return;

            _basePage.HideMenuAction = () =>
            {
                if (_dialogDrawer == null)
                    return;
                var r = _dialogDrawer.GetHidePosition();
                formsWrapper.Animate()
                    .X((float)r.left)
                    .Y((float)r.top)
                    .SetDuration(menu.AnimationDurationMillisecond)
                            .SetListener(new AnimatorListener(_dialogDrawer, false, _basePage))
                    .Start();
            };

            _basePage.ShowMenuAction = () =>
            {
                if (_dialogDrawer == null)
                    return;
                var r = _dialogDrawer.GetShowPosition();
                formsWrapper.Animate()
                    .X((float)r.left)
                    .Y((float)r.top)
                    .SetDuration(menu.AnimationDurationMillisecond)
                            .SetListener(new AnimatorListener(_dialogDrawer, true, _basePage))
                    .Start();
            };

            if (formsWrapper != null)
            {
                formsWrapper.RemoveFromParent();
            }

            formsWrapper = new FormsElementWrapper(Context, menu, this.Element);
            formsWrapper.SetBackgroundResource(Resource.Drawable.dialogBorder);
            formsWrapper.Renderer.View.SetBackgroundResource(Resource.Drawable.dialogBorder);

            var metrics = Resources.DisplayMetrics;
            var rootView = formsWrapper;


            _dialogDrawer = (formsWrapper.Renderer as SlideMenuDroidRenderer).GragGesture;

            if (_dialogDrawer == null)
                return;
            var rect = _dialogDrawer.GetHidePosition();

            menu.Layout(new Xamarin.Forms.Rectangle(
                rect.left / metrics.Density,
                rect.top / metrics.Density,
                (rect.right - rect.left) / metrics.Density,
                (rect.bottom - rect.top) / metrics.Density));

            rootView.Layout((int)rect.left, (int)rect.top, (int)rect.right, (int)rect.bottom);
            ViewGroup.AddView(rootView);
            ViewGroup.BringChildToFront(rootView);

            _dialogDrawer.NeedShowBackgroundView = (open, alpha) =>
            {
                if (open)
                    ShowBackgroundOverlay(alpha);
                else
                    HideBackgroundOverlay();
            };

        }

        void HideBackgroundOverlay()
        {
            try
            {
                if (_backgroundOverlay != null)
                {
                    RemoveView(_backgroundOverlay);
                    _backgroundOverlay.Dispose();
                    _backgroundOverlay = null;
                }
            }
            catch (ObjectDisposedException)
            {
                // On Android, _backgroundOverlay was not null but HAD been disposed. Attempting to remove it caused an ObjectDisposedException
                _backgroundOverlay = null;
                // Swallow this - let anything else cause a problem
            }
        }


        void ShowBackgroundOverlay(double alpha)
        {
            if (_basePage == null)
                return;
            var menu = _basePage.SlideMenu;
            if (menu == null)
                return;

            double value = (double)(alpha * _basePage.SlideMenu.BackgroundViewColor.A);
            if (_backgroundOverlay != null)
            {
                var color = _basePage.SlideMenu.BackgroundViewColor.ToAndroid();
                color.A = (Byte)(255 * value);
                _backgroundOverlay.SetBackgroundColor(color);
                return;
            }
            _backgroundOverlay = new global::Android.Widget.LinearLayout(Android.App.Application.Context);
            ViewGroup.AddView(_backgroundOverlay);
            _backgroundOverlay.SetBackgroundColor(_basePage.SlideMenu.BackgroundViewColor.ToAndroid());

            _backgroundOverlay.Touch -= HideMenu;
            _backgroundOverlay.Touch += HideMenu;
            var metrics = Resources.DisplayMetrics;
            _backgroundOverlay.Layout(
                0,
                0,
                (int)(ScreenSizeHelper.ScreenWidth * metrics.Density),
                (int)(ScreenSizeHelper.ScreenHeight * metrics.Density));

        }

        void HideMenu(object sender, Android.Views.View.TouchEventArgs e)
        {
            _basePage.HideMenu();
        }

    }

    class AnimatorListener : Java.Lang.Object, Android.Animation.Animator.IAnimatorListener
    {
        IDialogDrawer _dragGesture;
        bool _isShow;
        IMenuContainerPage _basePage;

        public AnimatorListener(IDialogDrawer dragGesture, bool isShow, IMenuContainerPage basePage)
        {
            _dragGesture = dragGesture;
            _isShow = isShow;
            _basePage = basePage;
        }

        public void OnAnimationCancel(Animator animation)
        {

        }

        public void OnAnimationEnd(Animator animation)
        {
            if (_isShow)
                _dragGesture.NeedShowBackgroundView.Invoke(true, 1);
            else
                _dragGesture.NeedShowBackgroundView.Invoke(false, 0);

            _basePage.OnAnimationCompleted();
        }

        public void OnAnimationRepeat(Animator animation)
        {

        }

        public void OnAnimationStart(Animator animation)
        {

        }
    }

}


using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using ImpressoApp.iOS.Renderers;
using ImpressoApp.Controls;
using BaseMvvmToolkit;
using System.ComponentModel;

[assembly: ExportRenderer(handler: typeof(MenuContainerPage), target: typeof(CustomDialogRenderer))]

namespace ImpressoApp.iOS.Renderers
{
    public class CustomDialogRenderer : PageRenderer
    {
        public CustomDialogRenderer()
        {
            _pageRenderer = this as PageRenderer;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            _basePage = e.NewElement as IMenuContainerPage;

            ScreenSizeHelper.ScreenHeight = UIScreen.MainScreen.Bounds.Height;
            ScreenSizeHelper.ScreenWidth = UIScreen.MainScreen.Bounds.Width;

            Element.PropertyChanged += Element_PropertyChanged;
        }

        void Element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SlideMenu")
            {
                LayoutMenu();
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!CheckPageAndMenu())
                return;
            //if (_basePage.SlideMenu.IsFullScreen)
            //    UIApplication.SharedApplication.KeyWindow.AddSubview(_menuOverlayRenderer.NativeView);
            //else
            //_pageRenderer.View.AddSubview(_menuOverlayRenderer.NativeView);
        }

        private void AddView()
        {
            if (_basePage.SlideMenu.IsFullScreen)
                UIApplication.SharedApplication.KeyWindow.AddSubview(_wrapper);
            else
                _pageRenderer.View.AddSubview(_wrapper);
        }

        private void RemoveView()
        {
            _wrapper.RemoveFromSuperview();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            //    if (_menuOverlayRenderer != null)
            //        _menuOverlayRenderer.NativeView.RemoveFromSuperview();
            //    HideBackgroundOverlay();
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            base.ViewWillTransitionToSize(toSize, coordinator);

            var menu = _basePage.SlideMenu;

            // this is used for rotation 
            double bigValue = UIScreen.MainScreen.Bounds.Height > UIScreen.MainScreen.Bounds.Width ? UIScreen.MainScreen.Bounds.Height : UIScreen.MainScreen.Bounds.Width;
            double smallValue = UIScreen.MainScreen.Bounds.Height < UIScreen.MainScreen.Bounds.Width ? UIScreen.MainScreen.Bounds.Height : UIScreen.MainScreen.Bounds.Width;
            if (toSize.Width < toSize.Height)
            {
                ScreenSizeHelper.ScreenHeight = bigValue;
                // this is used for mutiltasking
                ScreenSizeHelper.ScreenWidth = toSize.Width < smallValue ? toSize.Width : smallValue;
            }
            else
            {
                ScreenSizeHelper.ScreenHeight = smallValue;
                ScreenSizeHelper.ScreenWidth = toSize.Width < bigValue ? toSize.Width : bigValue;
            }

            if (_dialogDrawer == null)
                return;

            _dialogDrawer.UpdateLayoutSize(menu);
            var rect = _dialogDrawer.GetHidePosition();
            menu.Layout(new Xamarin.Forms.Rectangle(
                rect.left,
                rect.top,
                (rect.right - rect.left),
                (rect.bottom - rect.top)));
            _dialogDrawer.LayoutHideStatus();

        }

        PageRenderer _pageRenderer;

        IMenuContainerPage _basePage;
        FormsElementWrapper _wrapper;
        IDialogDrawer _dialogDrawer;

        bool CheckPageAndMenu()
        {
            if (_basePage != null && _basePage.SlideMenu != null)
                return true;
            else
                return false;
        }


        UIView _backgroundOverlay;

        void HideBackgroundOverlay()
        {
            if (_backgroundOverlay != null)
            {
                _backgroundOverlay.RemoveFromSuperview();
                _backgroundOverlay.Dispose();
                _backgroundOverlay = null;
            }
            _wrapper.Renderer?.NativeView?.EndEditing(true);
        }


        void ShowBackgroundOverlay(double alpha)
        {
            if (!CheckPageAndMenu())
                return;
            nfloat value = (nfloat)(alpha * _basePage.SlideMenu.BackgroundViewColor.A);
            if (_backgroundOverlay != null)
            {
                _backgroundOverlay.BackgroundColor = _basePage.SlideMenu.BackgroundViewColor.ToUIColor().ColorWithAlpha(value);
                return;
            }
            _backgroundOverlay = new UIView();
            _backgroundOverlay.BackgroundColor = _basePage.SlideMenu.BackgroundViewColor.ToUIColor().ColorWithAlpha(value);

            _backgroundOverlay.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                this._basePage.HideMenu();
            }));

            if (_basePage.SlideMenu.IsFullScreen)
            {
                _backgroundOverlay.Frame = new CGRect(UIApplication.SharedApplication.KeyWindow.Frame.Location, UIApplication.SharedApplication.KeyWindow.Frame.Size);
                UIApplication.SharedApplication.KeyWindow.InsertSubviewBelow(_backgroundOverlay, _wrapper);
            }
            else
            {
                _backgroundOverlay.Frame = new CGRect(_pageRenderer.View.Frame.Location, _pageRenderer.View.Frame.Size);
                _pageRenderer.View.InsertSubviewBelow(_backgroundOverlay, _wrapper);
            }
        }


        void LayoutMenu()
        {
            if (!CheckPageAndMenu())
                return;

            //// areadly add gesture
            //if (_dialogDrawer != null)
            //return;

            var menu = _basePage.SlideMenu;

            _dialogDrawer = new VerticalDrawer(menu);
            _dialogDrawer.RequestLayout = (l, t, r, b, density) =>
            {
                _wrapper.Frame = new CGRect(l, t, (r - l), (b - t));
                _wrapper.SetNeedsLayout();
            };
            _dialogDrawer.NeedShowBackgroundView = (open, alpha) =>
            {
                if (open)
                    ShowBackgroundOverlay(alpha);
                else
                    HideBackgroundOverlay();
            };

            _basePage.HideMenuAction = () =>
            {
                UIView.Animate(((double)menu.AnimationDurationMillisecond) / 1000, () => { _dialogDrawer.LayoutHideStatus(); }, () => { _basePage.OnAnimationCompleted(); });
            };

            _basePage.ShowMenuAction = () =>
            {
                UIView.Animate(((double)menu.AnimationDurationMillisecond) / 1000, () => { _dialogDrawer.LayoutShowStatus(); }, () => { _basePage.OnAnimationCompleted(); });
            };

            if (_wrapper != null)
            {
                RemoveView();
            }

            _wrapper = new FormsElementWrapper(menu, this.Element);
            AddView();

            var rect = _dialogDrawer.GetHidePosition();
            menu.Layout(new Rectangle(
                rect.left,
                rect.top,
                (rect.right - rect.left),
                (rect.bottom - rect.top)));
            _wrapper.Frame = new CGRect(
                rect.left,
                rect.top,
                (rect.right - rect.left),
                (rect.bottom - rect.top));
            _wrapper.SetNeedsLayout();

            _wrapper.Layer.CornerRadius = (nfloat)_basePage.SlideMenu.CornerRadius;
            _wrapper.Renderer.NativeView.Layer.CornerRadius = (nfloat)_basePage.SlideMenu.CornerRadius;
        }
    }
}


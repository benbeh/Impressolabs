using System;

namespace BaseMvvmToolkit
{
    public class VerticalDrawer : IDialogDrawer, IDisposable
    {
        protected double _left, _right, _top, _bottom = 0;
        private double _density;
        protected bool _willShown = true;

        public Action<double, double, double, double, double> RequestLayout
        {
            get;
            set;
        }

        public Action<bool, double> NeedShowBackgroundView
        {
            get;
            set;
        }

        double _topMax, _topMin, _bottomMax, _bottomMin = 0;
        bool _isToptoBottom = true;

        public VerticalDrawer(SlideCustomDialog view, double density = 1.0)
        {
            _density = density;
            view.GetIsShown = () =>
            {
                return !_willShown;
            };
            CheckViewBound(view);
            UpdateLayoutSize(view);
            view.HideEvent = LayoutHideStatus;
        }

        void CheckViewBound(SlideCustomDialog view)
        {
            if (ScreenSizeHelper.ScreenHeight == 0 || ScreenSizeHelper.ScreenWidth == 0)
                throw new Exception("Please set ScreenSizeHelper.ScreenHeight or ScreenSizeHelper.ScreenWidth");
            if (view.HeightRequest <= 0)
                throw new Exception("Please set SildeMenuView HeightRequest");
        }

        public void UpdateLayoutSize(SlideCustomDialog view)
        {
            _topMax = 0;
            _topMin = 0;
            _bottomMax = view.HeightRequest * _density;
            _bottomMin = 0;
            if (view.MenuOrientations == MenuOrientation.BottomToTop)
            {
                _isToptoBottom = false;
                _topMax = (ScreenSizeHelper.ScreenHeight) * _density;
                _topMin = (ScreenSizeHelper.ScreenHeight - view.HeightRequest - view.BottomMargin) * _density;
                _bottomMax = (ScreenSizeHelper.ScreenHeight + view.HeightRequest) * _density;
                _bottomMin = (ScreenSizeHelper.ScreenHeight - view.BottomMargin) * _density;
            }
            if (!view.IsFullScreen)
            {
                _left = view.LeftMargin * _density;
                _right = (view.LeftMargin + view.WidthRequest) * _density;
            }
            else
            {
                _left = view.LeftMargin * _density;
                _right = (ScreenSizeHelper.ScreenWidth - view.RightMargin) * _density;
            }
        }

        public void LayoutShowStatus()
        {
            if (RequestLayout != null)
            {
                GetShowPosition();
                RequestLayout(_left, _top, _right, _bottom, _density);
            }
            if (NeedShowBackgroundView != null)
                NeedShowBackgroundView(true, 1);
        }

        public void LayoutHideStatus()
        {
            if (RequestLayout != null)
            {
                GetHidePosition();
                RequestLayout(_left, _top, _right, _bottom, _density);
            }
            if (NeedShowBackgroundView != null)
                NeedShowBackgroundView(false, 0);
        }

        public Rect GetShowPosition()
        {
            _willShown = false;
            _top = _isToptoBottom ? _topMax : _topMin;
            _bottom = _isToptoBottom ? _bottomMax : _bottomMin;
            return new Rect()
            {
                left = _left,
                top = _isToptoBottom ? _topMax : _topMin,
                right = _right,
                bottom = _isToptoBottom ? _bottomMax : _bottomMin
            };
        }

        public Rect GetHidePosition()
        {
            _willShown = true;
            _top = _isToptoBottom ? _topMin : _topMax;
            _bottom = _isToptoBottom ? _bottomMin : _bottomMax;
            return new Rect()
            {
                left = _left,
                top = _isToptoBottom ? _topMin : _topMax,
                right = _right,
                bottom = _isToptoBottom ? _bottomMin : _bottomMax
            };
        }

        public void Dispose()
        {
            this.RequestLayout = null;
            this.NeedShowBackgroundView = null;
        }
    }
}


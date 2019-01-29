using System;

namespace BaseMvvmToolkit
{
    public struct Rect
    {
        public double left, top, right, bottom;

    }

    public interface IDialogDrawer
    {

        Action<double, double, double, double, double> RequestLayout { get; set; }

        void LayoutShowStatus();

        void LayoutHideStatus();

        Rect GetShowPosition();

        Rect GetHidePosition();

        void UpdateLayoutSize(SlideCustomDialog view);

        Action<bool, double> NeedShowBackgroundView { get; set; }
    }
}


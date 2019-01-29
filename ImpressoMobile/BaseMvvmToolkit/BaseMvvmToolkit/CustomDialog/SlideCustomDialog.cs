using System;

using Xamarin.Forms;

namespace BaseMvvmToolkit
{
    [Flags]
    public enum MenuOrientation
    {
        TopToBottom,
        BottomToTop,
        LeftToRight,
        RightToLeft,
    }

    public class SlideCustomDialog : ContentView
    {
        public SlideCustomDialog()
        {
            // It must set background color, otherwise it will cannot be dragged in Android 
            this.IsFullScreen = true;
            this.MenuOrientations = MenuOrientation.BottomToTop;
            this.BottomMargin = 10;
            this.RightMargin = 10;
            this.LeftMargin = 10;
            this.CornerRadius = 10;
            this.BackgroundViewColor = new Color(0, 0, 0, 0.5);
            this.BackgroundColor = Color.White;
        }

        public static readonly BindableProperty MenuOrientationsProperty = BindableProperty.Create(
                                                                               "MenuOrientations",
                                                                               typeof(MenuOrientation),
                                                                               typeof(SlideCustomDialog),
                                                                               MenuOrientation.TopToBottom);

        public MenuOrientation MenuOrientations
        {
            get
            {
                return (MenuOrientation)GetValue(MenuOrientationsProperty);
            }
            set
            {
                SetValue(MenuOrientationsProperty, value);
            }
        }

        public static readonly BindableProperty LeftMarginProperty = BindableProperty.Create(
                                                                         "LeftMargin",
                                                                         typeof(double),
                                                                         typeof(SlideCustomDialog),
                                                                         0.0);

        public double LeftMargin         {             get             {                 return (double)GetValue(LeftMarginProperty);             }             set             {                 SetValue(LeftMarginProperty, value);             }
         }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
                                                                 "CornerRadius",
                                                                 typeof(double),
                                                                 typeof(SlideCustomDialog),
                                                                 0.0);

        public double CornerRadius
        {
            get
            {
                return (double)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }          public static readonly BindableProperty RightMarginProperty = BindableProperty.Create(                                                                          "RightMargin",                                                                          typeof(double),             typeof(SlideCustomDialog),                                                                          0.0);          public double RightMargin         {             get             {                 return (double)GetValue(RightMarginProperty);             }             set             {                 SetValue(RightMarginProperty, value);             }         }          public static readonly BindableProperty TopMarginProperty = BindableProperty.Create(                                                                         "TopMargin",                                                                         typeof(double),             typeof(SlideCustomDialog),                                                                         0.0);          public double TopMargin         {             get             {                 return (double)GetValue(TopMarginProperty);             }             set             {                 SetValue(TopMarginProperty, value);             }         }          public static readonly BindableProperty BottomMarginProperty = BindableProperty.Create(             "BottomMargin",                                                                 typeof(double),             typeof(SlideCustomDialog),                                                                 0.0);          public double BottomMargin         {             get             {                 return (double)GetValue(BottomMarginProperty);             }             set             {                 SetValue(BottomMarginProperty, value);             }         }  

        public static readonly BindableProperty IsFullScreenProperty = BindableProperty.Create(
                                                                           "IsFullScreen",
                                                                           typeof(bool),
                                                                           typeof(SlideCustomDialog),
                                                                           false);

        public bool IsFullScreen
        {
            get
            {
                return (bool)GetValue(IsFullScreenProperty);
            }
            set
            {
                SetValue(IsFullScreenProperty, value);
            }
        }

        public static readonly BindableProperty AnimationDurationMillisecondProperty = BindableProperty.Create(
                                                                                           "AnimationDurationMillisecond",
                                                                                           typeof(int),
                                                                                           typeof(SlideCustomDialog),
                                                                                           250);

        public int AnimationDurationMillisecond
        {
            get
            {
                return (int)GetValue(AnimationDurationMillisecondProperty);
            }
            set
            {
                SetValue(AnimationDurationMillisecondProperty, value);
            }
        }

        public static readonly BindableProperty BackgroundViewColorProperty = BindableProperty.Create(
                                                                                  "BackgroundViewColor",
                                                                                  typeof(Color),
                                                                                  typeof(SlideCustomDialog),
                                                                                  Color.Gray);

        public Color BackgroundViewColor
        {
            get
            {
                return (Color)GetValue(BackgroundViewColorProperty);
            }
            set
            {
                SetValue(BackgroundViewColorProperty, value);
            }
        }

        internal Action HideEvent { get; set; }

        public void HideWithoutAnimations()
        {
            if (HideEvent != null)
                HideEvent();
        }

        public bool IsShown
        {
            get
            {
                if (GetIsShown == null)
                    return false;
                else
                    return GetIsShown();
            }
        }

        internal Func<bool> GetIsShown { get; set; }
    }
}



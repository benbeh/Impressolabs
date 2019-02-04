using System;
using Android.Content.Res;

namespace ImpressoApp.Droid
{
    public static class LayoutUtils
    {
        public static int ToDp(this int px)         {             return ToDp((float)px);         }          public static int ToDp(this float px)         {             return (int)(px / Resources.System.DisplayMetrics.Density);         }          public static int ToPx(this int dp)         {             return (int)(dp * Resources.System.DisplayMetrics.Density);         }
    }
}

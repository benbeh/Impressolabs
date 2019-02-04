using System;
using Android.Content;

namespace ImpressoApp.Droid.Utils
{
    public static class AndroidUtils
    {
        public static float ConvertDpToPixel(Context context, float dp)
        {
            return dp * context.Resources.DisplayMetrics.Density;
        }
    }
}

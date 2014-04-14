using System;
using Android.Util;
using Android.Content;

namespace Droid
{
    public class UIHelper
    {
        public static int DpToPixels(Context context, int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }
}


using System;
using Android.Util;
using Android.Content;

namespace PerpetualEngine.Droid
{
    public class UIHelper
    {
        static Context context;

        public static void SetContext(Context c)
        {
            context = c;
        }

        public static int DpToPixels(int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }
}


using System;
using Android.Util;
using Android.Content;

namespace PerpetualEngine.Droid
{
    public class UIHelper
    {
        static Context context;

        public static void SetContext(Context context)
        {
            UIHelper.context = context;
        }

        public static int DpToPixels(int dp)
        {
            if (context == null)
                throw new InvalidOperationException("please provide a context");
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }
}


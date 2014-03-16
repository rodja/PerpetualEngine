using System;
using Android.Content;
using Android.Util;
using Android.Views;

namespace Sipt.Droid
{
    public static class HelpfulExtensions
    {
        public static void SetPaddingDp(this ViewGroup viewGroup, int left, int top, int right, int bottom)
        {
            var context = viewGroup.Context;
            viewGroup.SetPadding(
                context.DpToPixels(left),
                context.DpToPixels(top),
                context.DpToPixels(right),
                context.DpToPixels(bottom)
            );
        }

        public static int DpToPixels(this Context context, int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }
}


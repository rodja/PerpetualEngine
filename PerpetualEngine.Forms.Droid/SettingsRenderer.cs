using System;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Xamarin.Forms;
using Android.Views;
using Android.App;

namespace PerpetualEngine.Forms
{
    abstract public class SettingsRenderer :ViewCellRenderer
    {
        protected override global::Android.Views.View GetCellCore(Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var view = base.GetCellCore(item, convertView, parent, context) as ViewGroup;

            (item as Setting).RendererTapAction = delegate {
                var dialog = CreateDialog(context);
                dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
                dialog.Show();
            };
            return view;
        }

        abstract protected Dialog CreateDialog(Context context);
    }
}


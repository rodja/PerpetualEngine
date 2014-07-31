using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PerpetualEngine.Forms;

[assembly: ExportRenderer(typeof(SelectionSetting), typeof(SelectionSettingsRenderer))]

namespace PerpetualEngine.Forms
{
    public class SelectionSettingsRenderer : ViewCellRenderer
    {
        protected override global::Android.Views.View GetCellCore(Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
        {
            (item as Setting).TapAction = delegate {
                ShowSelectionDialog(context);
            };

            return base.GetCellCore(item, convertView, parent, context);
        }

        void ShowSelectionDialog(Context context)
        {
            var setting = Cell as SelectionSetting;
            var builder = new AlertDialog.Builder(context);
            builder.SetTitle(setting.Title.Text);
            var options = setting.Options;
            builder.SetItems(options.Values.ToArray<string>(), new SelectionApplier {
                Values = options.Keys.ToList<string>(),
                Setting = setting
            });
            builder.Create().Show();
        }

        public class SelectionApplier: Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            public List<string> Values;
            public SelectionSetting Setting;

            public void OnClick(IDialogInterface dialog, int which)
            {
                Setting.Value = Values[which];
            }
        }

    }
}

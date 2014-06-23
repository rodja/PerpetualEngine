using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views;
using Android.App;
using Android.Content;
using System.Collections.Generic;
using System.Linq;

[assembly: ExportRenderer(typeof(TextCell), typeof(PerpetualEngine.Forms.ImprovedTextCellRenderer))]
[assembly: ExportRenderer(typeof(PerpetualEngine.Forms.SelectionSetting), typeof(PerpetualEngine.Forms.SelectionSettingRenderer))]

namespace PerpetualEngine.Forms
{
    public class ImprovedTextCellRenderer : TextCellRenderer
    {
        protected override global::Android.Views.View GetCellCore(Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var view = base.GetCellCore(item, convertView, parent, context) as ViewGroup;
            if (String.IsNullOrEmpty((item as TextCell).Text)) {
                view.Visibility = ViewStates.Gone;
                while (view.ChildCount > 0)
                    view.RemoveViewAt(0);
                view.SetMinimumHeight(0);
                view.SetPadding(0, 0, 0, 0);
            }
            return view;
        }
    }

    public class SelectionSettingRenderer : SettingsRenderer
    {

        override protected Dialog CreateDialog(Context context)
        {
            var setting = Cell as SelectionSetting;
            var builder = new AlertDialog.Builder(context);
            builder.SetTitle(setting.Title);
            var options = setting.Options;
            builder.SetItems(options.Values.ToArray<string>(), new SelectionApplier {
                Values = options.Keys.ToList<string>(),
                Setting = setting
            });
            return builder.Create();
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


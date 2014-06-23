using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views;
using Android.App;
using Android.Content;
using System.Collections.Generic;
using System.Linq;
using PerpetualEngine.Forms;

[assembly: ExportRenderer(typeof(SelectionSetting), typeof(SelectionSettingsRenderer))]

namespace PerpetualEngine.Forms
{

    public class SelectionSettingsRenderer : SettingsRenderer
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

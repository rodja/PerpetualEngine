using System;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Content;
using Android.App;
using Android.Widget;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PerpetualEngine.Forms.TextSetting), typeof(PerpetualEngine.Forms.TextSettingRenderer))]

namespace PerpetualEngine.Forms
{
    public class TextSettingRenderer : SettingsRenderer
    {
        override protected Dialog CreateDialog(Context context)
        {
            var setting = Cell as TextSetting;
            var builder = new AlertDialog.Builder(context);
            builder.SetTitle(setting.Value);
            var input = new EditText(context);
            input.Text = setting.Value;
            input.SelectAll();
            builder.SetView(input);
            builder.SetPositiveButton("Ok", new TextApplier(input, setting));
            var dialog = builder.Create();
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            return dialog;
        }

        class TextApplier: Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            EditText input;
            TextSetting setting;

            public TextApplier(EditText input, TextSetting setting)
            {
                this.input = input;
                this.setting = setting;
            }

            public void OnClick(IDialogInterface dialog, int which)
            {
                setting.Value = input.Text;
            }
        }

       
    }


}


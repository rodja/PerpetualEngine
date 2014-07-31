using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PerpetualEngine.Forms;

[assembly: ExportRenderer(typeof(TextSetting), typeof(TextSettingRenderer))]

namespace PerpetualEngine.Forms
{
    public class TextSettingRenderer : ViewCellRenderer
    {
        protected override global::Android.Views.View GetCellCore(Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
        {
            (item as Setting).TapAction = delegate {
                ShowTextDialog(context);
            };

            return base.GetCellCore(item, convertView, parent, context);
        }

        void ShowTextDialog(Context context)
        {
            var setting = Cell as TextSetting;
            var builder = new AlertDialog.Builder(context);
            builder.SetTitle(setting.Title.Text);
            var input = new EditText(context);
            input.Text = setting.Value;
            input.SelectAll();
            builder.SetView(input);
            builder.SetPositiveButton("Ok", new TextApplier(input, setting));
            var dialog = builder.Create();
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.Show();
        }

        class TextApplier: Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            readonly EditText input;
            readonly TextSetting setting;

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

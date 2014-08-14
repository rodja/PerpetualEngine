using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PerpetualEngine.Forms;

[assembly: ExportRenderer(typeof(SelectionSetting), typeof(SelectionSettingsRenderer))]

namespace PerpetualEngine.Forms
{
    public class SelectionSettingsRenderer : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
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
            AlertDialog dialog = null;
            var viewGroup = new TableLayout(context);
            if (setting.FreeText) {
                var editText = new EditText(context) {
                    Text = setting.Value,
                };
                editText.SetSingleLine();
                editText.SetSelectAllOnFocus(true);
                editText.EditorAction += (sender, e) => {
                    if (e.ActionId == ImeAction.Done) {
                        setting.Value = editText.Text;
                        dialog.Dismiss();
                    }
                };
                viewGroup.AddView(editText);
            }
            for (var i = 0; i < options.Values.Count; i++) {
                var view = new TextView(context) {
                    Text = options.Values.ElementAt(i),
                    Tag = options.Keys.ElementAt(i),
                    TextSize = 20,
                };
                view.SetPadding(16, 16, 16, 16);
                viewGroup.AddView(view);
                view.Touch += (sender, e) => {
                    Console.WriteLine(view.Text + " clicked");
                    view.SetBackgroundColor(Android.Graphics.Color.DarkGray);
                    setting.Value = view.Tag.ToString();
                    (new Handler()).PostDelayed(delegate {
                        dialog.Dismiss();
                    }, 100);
                };
            }
            builder.SetView(viewGroup);
            dialog = builder.Create();
            if (setting.FreeText)
                dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.Show();
        }
    }
}

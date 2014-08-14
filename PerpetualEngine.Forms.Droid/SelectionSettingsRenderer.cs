using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PerpetualEngine.Forms;
using Android.Widget;
using System;
using Android.OS;

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
            var editText = new EditText(context);
            viewGroup.AddView(editText);
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

//            builder.SetItems(options.Values.ToArray<string>(), new SelectionApplier {
//                Values = options.Keys.ToList<string>(),
//                Setting = setting
//            });

            dialog = builder.Create();
            dialog.Show();
        }

        //        public class SelectionApplier: Java.Lang.Object, IDialogInterfaceOnClickListener
        //        {
        //            public List<string> Values;
        //            public SelectionSetting Setting;
        //
        //            public void OnClick(IDialogInterface dialog, int which)
        //            {
        //                Setting.Value = Values[which];
        //            }
        //        }

    }
}

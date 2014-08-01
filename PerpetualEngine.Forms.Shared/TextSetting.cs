using Xamarin.Forms;

namespace PerpetualEngine.Forms
{
    public class TextSetting: Setting
    {
        readonly Entry entry;

        public TextSetting(string title, string key = null) : base(title, key)
        {
            if (Device.OS == TargetPlatform.iOS) {
                Title.IsVisible = false;
                Description.WidthRequest = 0;
                entry = new Entry {
                    Placeholder = Title.Text,
                    Text = Value,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                entry.Completed += delegate {
                    Value = entry.Text;
                };
                (View as StackLayout).Children.Add(entry);
            }
        }

        public override string Value {
            get {
                return base.Value;
            }
            set {
                base.Value = value;

                if (entry != null)
                    entry.Text = value;
            }
        }
    }
}

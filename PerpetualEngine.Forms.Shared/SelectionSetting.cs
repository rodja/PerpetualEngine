using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PerpetualEngine.Forms
{
    public class SelectionSetting: Setting
    {
        public IDictionary<string, string> Options = new {}.ToOptions();

        public SelectionSetting(string key, string title) : base(key, title)
        {
            if (Device.OS == TargetPlatform.iOS)
                RendererTapAction = delegate {
                    var listView = new ListView {
                        ItemsSource = Options.Values,
                    };
                    listView.ItemTapped += (sender, e) => {
//                    TODO
                    };
                    ParentView.Navigation.PushAsync(new ContentPage {
                        Content = listView,
                    });
                };
        }

        public override string Value {
            get {
                return base.Value;
            }
            set {
                if (Options.Count == 0) {
                    base.Value = "";
                    Description.Text = "";
                } else {
                    if (!Options.ContainsKey(value))
                        value = Options.Keys.First();
                    base.Value = value;
                    Description.Text = Options[value];
                }
            }
        }

    }
    
}

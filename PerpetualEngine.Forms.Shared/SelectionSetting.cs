using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PerpetualEngine.Forms
{
    public class SelectionSetting: Setting
    {
        public IDictionary<string, string> Options = new {}.ToOptions();

        public SelectionSetting(string title, string key = null) : base(title, key)
        {
            if (Device.OS == TargetPlatform.iOS)
                Tapped += delegate {
                    ShowOptionsList();
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

        void ShowOptionsList()
        {
            var optionList = new List<Option>();
            foreach (var key in Options.Keys)
                optionList.Add(new Option{ Value = Options[key], Key = key });
            var listView = new ListView {
                ItemsSource = optionList,
                ItemTemplate = new DataTemplate(typeof(OptionCell)),
            };
            listView.ItemTapped += (sender, e) => {
                Value = (listView.SelectedItem as Option).Key;
                ParentView.Navigation.PopAsync();
            };
            ParentView.Navigation.PushAsync(new ContentPage {
                Content = listView,
            });
        }

        class OptionCell: TextCell
        {
            public OptionCell()
            {
                this.SetBinding(TextCell.TextProperty, "Value");
            }
        }

        class Option
        {
            public string Value{ get; set; }

            public string Key{ get; set; }
        }
    }
    
}

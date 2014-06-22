using System;
using Xamarin.Forms;
using PerpetualEngine.Storage;
using System.Collections.Generic;

namespace PerpetualEngine.Forms
{

    public class Setting : ViewCell
    {
        SimpleStorage storage;

        public string Title { get; private set; }

        protected string DefaultValue;

        protected Label Description;

        public Setting(string title, string defaultValue = "")
        {
            this.Title = title;
            this.DefaultValue = defaultValue;

            storage = SimpleStorage.EditGroup("settings");

            Tapped += delegate {
                RendererTapAction();
            };

            var view = new StackLayout();
            view.Padding = 8;
            view.Spacing = 0;
            view.Children.Add(new Label {
                Text = title,
                Font = Font.SystemFontOfSize(NamedSize.Large),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.White
            });
            Description = new Label();
            view.Children.Add(Description);
            View = view;
        }

        protected override void OnAppearing()
        {
            // apply current stored value to the ui
            Value = storage.Get(Title, DefaultValue);
            base.OnAppearing();
        }

        public Action RendererTapAction;

        virtual public string Value {
            get {
                return storage.Get(Title, DefaultValue);
            }
            set {
                Description.Text = value;
                storage.Put(Title, value);

                Description.IsVisible = DescriptionVisible;
            }
        }

        protected virtual bool DescriptionVisible {
            get {
                return !String.IsNullOrEmpty(Value);
            }
        }
    }

    public class TextSetting: Setting
    {
        public TextSetting(string title, string defaultValue) : base(title, defaultValue)
        {
        }
    }

    public class SelectionSetting: Setting
    {
        public Dictionary<string, string> Options;

        public SelectionSetting(string title, string defaultValue = "") : base(title, defaultValue)
        {
        }

        public override string Value {
            get {
                return base.Value;
            }
            set {
                if (!Options.ContainsKey(value))
                    value = DefaultValue;
                base.Value = value;
                Description.Text = Options[value];
            }
        }

    }

    public class SwitchSetting: Setting
    {
        public SwitchSetting(string title, bool defaultValue = false) : base(title, defaultValue ? "on" : "off")
        {
            var layout = (View as StackLayout);
            layout.Orientation = StackOrientation.Horizontal;
            var toggle = new Switch();
            toggle.Toggled += (sender, e) => {
                Value = e.Value ? "on" : "off";
            };
            toggle.IsToggled = Value == "on";
            layout.Children.Add(toggle);

            Tapped += delegate {
                toggle.IsToggled = !toggle.IsToggled;
            };
        }

        protected override bool DescriptionVisible {
            get {
                return false;
            }
        }

    }
}
    


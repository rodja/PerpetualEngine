using System;
using PerpetualEngine.Storage;
using Xamarin.Forms;

namespace PerpetualEngine.Forms
{
    public abstract class Setting : ViewCell
    {
        SimpleStorage storage;

        public string Key { get; private set; }

        public Label Title { get; private set; }

        protected Label Description;

        bool deleteStorageOnDisappearing;

        public Action TapAction = delegate {
        };

        protected Setting(string title, string key = null)
        {
            storage = SimpleStorage.EditGroup("settings");
            Key = key;

            Title = new Label {
                Text = title,
                Font = Device.OnPlatform<Font>(
                    Font.SystemFontOfSize(NamedSize.Medium),
                    Font.SystemFontOfSize(NamedSize.Large),
                    Font.SystemFontOfSize(NamedSize.Medium)
                ),
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            Description = new Label {
                TextColor = Device.OS == TargetPlatform.iOS ? Color.Gray : Color.Black,
            };

            View = new StackLayout {
                Orientation = Device.OS == TargetPlatform.iOS ? StackOrientation.Horizontal : StackOrientation.Vertical,
                Children = {
                    Title,
                    Description,
                },
                Padding = 8,
                Spacing = 0,
            };
            Tapped += delegate {
                TapAction();
            };
        }

        protected override void OnAppearing()
        {
            if (Key == null) {
                Key = Guid.NewGuid().ToString();
                deleteStorageOnDisappearing = true;
            }
            Value = storage.Get(Key, "");

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            if (deleteStorageOnDisappearing)
                storage.DeleteAsync(Key);

            base.OnDisappearing();
        }

        virtual public string Value {
            get {
                return storage.Get(Key, "");
            }
            set {
                var changed = !value.Equals(Value, StringComparison.Ordinal);

                Description.Text = value;
                storage.Put(Key, value);

                Description.IsVisible = DescriptionVisible;

                if (changed)
                    OnValueChanged(this, new EventArgs());
            }
        }

        protected virtual bool DescriptionVisible {
            get {
                return !String.IsNullOrEmpty(Value);
            }
        }

        public event EventHandler OnValueChanged = delegate {};
    }
}
    


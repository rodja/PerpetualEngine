using System;
using PerpetualEngine.Storage;
using Xamarin.Forms;

namespace PerpetualEngine.Forms
{
    public abstract class Setting : ViewCell
    {
        SimpleStorage storage;

        public string Title { get; private set; }

        public string Key { get; private set; }

        protected Label Description;

        bool deleteStorageOnDisappearing;

        public Action TapAction = delegate {
        };

        protected Setting(string title, string key = null)
        {
            Key = key;
            Title = title;

            storage = SimpleStorage.EditGroup("settings");

            var view = new StackLayout();
            view.Padding = 8;
            view.Spacing = 0;
            view.Children.Add(new Label {
                Text = title,
                Font = Device.OnPlatform<Font>(
                    Font.SystemFontOfSize(NamedSize.Medium),
                    Font.SystemFontOfSize(NamedSize.Large),
                    Font.SystemFontOfSize(NamedSize.Medium)
                ),
                HorizontalOptions = LayoutOptions.FillAndExpand,
            });
            Description = new Label();
            if (Device.OS == TargetPlatform.iOS) {
                view.Orientation = StackOrientation.Horizontal;
                Description.TextColor = Color.Gray;
            }
            view.Children.Add(Description);
            Tapped += delegate {
                TapAction();
            };
            View = view;
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
    


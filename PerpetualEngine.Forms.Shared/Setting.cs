using Xamarin.Forms;
using PerpetualEngine.Storage;
using System;

namespace PerpetualEngine.Forms
{

    public class Setting : ViewCell
    {
        SimpleStorage storage;

        public string Title { get; private set; }

        public string Key { get; private set; }

        protected Label Description;

        public Setting(string key, string title)
        {
            Key = key;
            this.Title = title;

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
            Value = storage.Get(Key);
            base.OnAppearing();
        }

        public Action RendererTapAction;

        virtual public string Value {
            get {
                return storage.Get(Key);
            }
            set {
                Description.Text = value;
                storage.Put(Key, value);

                Description.IsVisible = DescriptionVisible;
            }
        }

        protected virtual bool DescriptionVisible {
            get {
                return !String.IsNullOrEmpty(Value);
            }
        }
    }
}
    


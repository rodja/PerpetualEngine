using Xamarin.Forms;
using PerpetualEngine.Forms;

namespace PerpetualEngine.Demo
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Title = "Perpetual Engine";

            var text1 = new TextSetting("Text 1", "text1");
            text1.OnValueChanged += delegate {
                Msg.Log(this, "text1 changed");
            };

            var selection1 = new SelectionSetting("Selection 1", "selection1") {
                Options = new { id1 = "banana", id2 = "apple", id3 = "cookies" }.ToOptions(),
            };
            selection1.OnValueChanged += delegate {
                Msg.Log(this, "selection1 changed");
            };

            var selection2 = new SelectionSetting("Selection 2 (no options)", "selection2");

            var switch1 = new SwitchSetting("Switch 1 (persistent)", "switch1");
            switch1.OnValueChanged += delegate {
                Msg.Log(this, "switch1 changed");
                selection2.Options = new {a = "A", b = "B"}.ToOptions();
            };
            var switch2 = new SwitchSetting("Switch 2 (not persistent)");

            Content = new StackLayout {
                Orientation = StackOrientation.Vertical,
                Children = {
                    new TableView {
                        HasUnevenRows = true,
                        Root = new TableRoot {
                            new TableSection {
                                text1,
                                selection1,
                                selection2,
                                switch1,
                                switch2,
                            },
                        },
                    },
                },
            };
        }
    }

}


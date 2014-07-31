using Xamarin.Forms;
using PerpetualEngine.Forms;

namespace PerpetualEngine.Demo
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Title = "Perpetual Engine";

            var textSetting1 = new TextSetting("Text Setting 1", "textSetting1");
            textSetting1.OnValueChanged += delegate {
                Msg.Log(this, "textSetting1 changed");
            };

            var selectionSetting1 = new SelectionSetting("Selection Setting 1", "selectionSetting1") {
                Options = new { id1 = "banana", id2 = "apple", id3 = "cookies" }.ToOptions(),
            };
            selectionSetting1.OnValueChanged += delegate {
                Msg.Log(this, "selectionSetting1 changed");
            };

            var selectionSetting2 = new SelectionSetting("Selection Setting 2", "selectionSetting2");

            var switchSetting1 = new SwitchSetting("Switch Setting 1 (persistent)", "switchSetting1");
            switchSetting1.OnValueChanged += delegate {
                Msg.Log(this, "switchSetting1 changed");
                selectionSetting2.Options = new {a = "A", b = "B"}.ToOptions();
            };
            var switchSetting2 = new SwitchSetting("Switch Setting 2 (not persistent)");

            Content = new StackLayout {
                Orientation = StackOrientation.Vertical,
                Children = {
                    new TableView {
                        HasUnevenRows = true,
                        Root = new TableRoot {
                            new TableSection {
                                textSetting1,
                                selectionSetting1,
                                selectionSetting2,
                                switchSetting1,
                                switchSetting2,
                            },
                        },
                    },
                },
            };
        }
    }

}


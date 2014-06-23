using System;
using Xamarin.Forms;
using PerpetualEngine.Forms;
using System.Collections.Generic;

namespace PerpetualEngine.Demo
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Title = "Perpetual Engine";

            Content = new TableView {
                HasUnevenRows = true,
                Root = new TableRoot() {
                    new TableSection {
                        new SwitchSetting("demo_switch", "Demo Switch"),
                        new TextSetting("username", "Username Input Example"),
                        new SelectionSetting("fruit", "Which Fruit?") { Options = new { id1 = "banana", id2 = "apple", id3 = "cookies" }.ToOptions()
                        },
                        new SelectionSetting("cars", "Which Car?"){ Options = new Dictionary<string, string>() },
                    }
                }
            };
        }
    }

}


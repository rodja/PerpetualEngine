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
                Root = new TableRoot() {
                    new TableSection {
                        new SwitchSetting("demo_switch", "Demo Switch"),
                        new TextSetting("username", "Username Input Example"),
                        new SelectionSetting("fruit", "Which Fruit?") { Options = new Dictionary<string, string> { {
                                    "id_1",
                                    "apple"
                                }, {
                                    "id_2",
                                    "banana"
                                }
                            }
                        }
                    }
                }
            };
        }
    }

}


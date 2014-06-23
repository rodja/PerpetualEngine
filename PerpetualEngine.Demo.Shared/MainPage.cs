using System;
using Xamarin.Forms;
using PerpetualEngine.Forms;

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
                        new SwitchSetting("demo_switch", "Demo Switch")
                    }
                }
            };
        }
    }

}


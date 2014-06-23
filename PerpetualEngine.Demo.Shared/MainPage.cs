using System;
using Xamarin.Forms;

namespace PerpetualEngine.Demo
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Content = new TableView {
                Root = new TableRoot {
                    new TableSection {

                    }
                }
            };
        }
    }

}


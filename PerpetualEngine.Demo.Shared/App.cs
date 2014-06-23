using System;
using Xamarin.Forms;

namespace PerpetualEngine.Demo
{
    public class App
    {
        public static Page GetMainPage()
        {   
            return new NavigationPage(new MainPage());
        }
    }
}


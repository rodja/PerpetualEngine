using Xamarin.Forms;

namespace PerpetualEngine.Demo
{
    public static class App
    {
        public static Page GetMainPage()
        {   
            return new NavigationPage(new MainPage());
        }
    }
}

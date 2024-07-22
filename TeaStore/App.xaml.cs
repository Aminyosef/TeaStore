using TeaStore.Pages;

namespace TeaStore
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           var accesstoken = Preferences.Get("accessToken", string.Empty);
            if (string.IsNullOrEmpty(accesstoken))
            {
                MainPage = new NavigationPage(new SignupPage());
            }
            else
            {
                MainPage = new AppShell();
            }
        }
    }
}

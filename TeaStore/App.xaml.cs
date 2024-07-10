using TeaStore.Pages;

namespace TeaStore
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SignupPage();
        }
    }
}

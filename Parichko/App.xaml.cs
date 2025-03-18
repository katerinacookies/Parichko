using Parichko.Views;

namespace Parichko
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            if (UserIsLoggedIn())
            {
                Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                Shell.Current.GoToAsync("///MainPage");
            }
        }

        private bool UserIsLoggedIn()
        {
            var userId = Preferences.Get("LoggedUserId", null);
            if(userId != null)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}

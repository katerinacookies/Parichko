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
                if(Preferences.Get("LoggedUserName", "") == "Admin")
                {
                    Shell.Current.GoToAsync("///AdminHomePage");
                }
                //MainPage = new HomePage();
                else
                {
                    Shell.Current.GoToAsync("///HomePage");
                }
                
            }
            else
            {
                //MainPage = new MainPage();
                Shell.Current.GoToAsync("///MainPage");
            }
        }

        private bool UserIsLoggedIn()
        {
            int userId = Preferences.Get("LoggedUserId", 0);
            if(userId != 0)
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

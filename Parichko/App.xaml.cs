namespace Parichko
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (UserIsLoggedIn())
            {
                //MainPage = new QuestionsVisualDifficulties();
            }
            else
            {
                MainPage = new AppShell();
            }
        }

        private bool UserIsLoggedIn()
        {
            // 
            return false;
        }
    }
}

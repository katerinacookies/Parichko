namespace Parichko.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Login");
        }
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (Shell.Current == null)
            {
                await DisplayAlert("Error", "Shell.Current is null!", "OK");
                return;
            }

            await Shell.Current.GoToAsync("Register");
        }
    }

}

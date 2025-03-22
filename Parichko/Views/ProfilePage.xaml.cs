namespace Parichko.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		ShowName();
	}

	public async Task ShowName()
	{
		string name = Preferences.Get("LoggedUserName", "");
		displayName.Text = name;
		string email = Preferences.Get("LoggedUserEmail", "");
		displayEmail.Text = email;
	}

	public async void OnLogoutClicked(object sender, EventArgs e)
	{
		RestartShell();
        await Shell.Current.GoToAsync("//MainPage?refresh=true");

	}
	private static async void RestartShell()
	{
        Preferences.Clear();
		//Application.Current.MainPage = new AppShell();
    }
}
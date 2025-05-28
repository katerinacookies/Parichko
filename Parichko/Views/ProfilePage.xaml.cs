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
    public async void OnEditClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///EditProfilePage");
    }
    public async void OnHelpClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///HelpPage");
    }
    public async void OnFriendsClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.Write("Бутонът е натиснат.");
        await Shell.Current.GoToAsync("///FriendsPage");

    }
    public async void OnCategoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AddCategoryPage");

    }
    public async void OnGoalClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AddGoalPage");

    }
    public async void OnIncomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AddIncomePage");

    }
    public async void OnExpenseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AddExpensePage");

    }
    private static void RestartShell()
	{
        Preferences.Clear();
		//Application.Current.MainPage = new AppShell();
    }
}
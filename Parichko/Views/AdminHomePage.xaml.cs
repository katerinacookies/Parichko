using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AdminHomePage : ContentPage
{
	private readonly AdminHomePageViewModel _viewModel;
	public AdminHomePage(AdminHomePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
		ShowLoadedUsers();
	}

	private async void ShowLoadedUsers()
	{
		await _viewModel.LoadUsersAsync();
	}

    public async void OnLogoutClicked(object sender, EventArgs e)
    {
		Preferences.Clear();
        await Shell.Current.GoToAsync("//MainPage?refresh=true");
    }
}
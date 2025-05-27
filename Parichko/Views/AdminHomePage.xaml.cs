using Parichko.Models;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AdminHomePage : ContentPage
{
	private readonly AdminHomePageViewModel _viewModel;
    private int deleteClicked = 0;
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

    public async void OnDeleteClicked(object sender, EventArgs e)
    {
        deleteClicked++;
        if (deleteClicked == 1)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.DisplayAlert("Внимание!", "Всички данни на този профил ще бъдат изтрити. Натиснете втори път за изтриване.", "Разбрах");
            });

        }
        if (deleteClicked == 2)
        {
            if (sender is Button button && button.BindingContext is UserProfile profile)
            {
                int upId = profile.Id;
                await _viewModel.DeleteProfile(upId);
            }
        }
    }
}
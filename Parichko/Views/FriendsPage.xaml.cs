using Parichko.ViewModels;
using System.Globalization;

namespace Parichko.Views;

public partial class FriendsPage : ContentPage
{
    private readonly FriendViewModel _viewModel;
    public FriendsPage(FriendViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        ShowRequests();
        ShowFriends();
    }

    private async void ShowRequests()
    {
        await _viewModel.LoadRequestsAsync();
    }
    private async void ShowFriends()
    {
        await _viewModel.LoadFriendsAsync();
    }

    public async void OnAddFriendClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text.ToString();
        await _viewModel.AddFriend(email);
    }
    public async void OnBackClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///ProfilePage");
	}
}
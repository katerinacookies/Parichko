using DataAccess.Models;
using Parichko.Models;
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
        EmailEntry.Text = string.Empty;
    }
    public async void OnBackClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///ProfilePage");
	}

    public async void OnDenyRequestClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is FriendRequest request)
        {
            int fromUserId = request.FromUser.Id;
            await _viewModel.DenyRequest(fromUserId);
        }
    }
    public async void OnAcceptRequestClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is FriendRequest request)
        {
            int fromUserId = request.FromUser.Id;
            await _viewModel.AcceptRequest(fromUserId);
        }
    }
    public async void OnRemoveFriendClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is UserProfile friend)
        {
            int friendId = friend.Id;
            await _viewModel.RemoveFriend(friendId);
        }
    }
}
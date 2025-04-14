namespace Parichko.Views;

public partial class FriendsPage : ContentPage
{
	public FriendsPage()
	{
		InitializeComponent();
	}

	public async void OnBackClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///ProfilePage");
	}
}
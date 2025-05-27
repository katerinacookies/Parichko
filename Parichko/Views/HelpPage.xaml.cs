namespace Parichko.Views;

public partial class HelpPage : ContentPage
{
	public HelpPage()
	{
		InitializeComponent();
	}
	public async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ProfilePage");
    }
}
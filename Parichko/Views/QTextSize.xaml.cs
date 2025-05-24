namespace Parichko.Views;

public partial class QTextSize : ContentPage
{
	public QTextSize()
	{
		InitializeComponent();
	}

    private async void OnForwardBtnClicked(object sender, EventArgs e)
    {
        //string displayname = UsernameEntry.Text.ToString();
        //await _viewModel.SetDisplayNameAsync(displayname);
        await Shell.Current.GoToAsync("///AllDonePage?refresh=true");
    }
}
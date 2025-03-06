namespace Parichko.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
		Displaythename();
	}

	public async Task Displaythename()
	{
		string name = Preferences.Get("LoggedUserName", null);
		displayhi.Text = "Здравей, " + name;
	}
}
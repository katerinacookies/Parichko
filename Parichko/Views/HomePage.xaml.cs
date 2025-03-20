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
		//? await
		string name = Preferences.Get("LoggedUserName", "");
		displayhi.Text = "Здравей, " + name;
	}
}
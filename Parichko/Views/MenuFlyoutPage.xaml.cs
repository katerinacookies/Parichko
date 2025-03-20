namespace Parichko.Views;

public partial class MenuFlyoutPage : FlyoutPage
{
	public MenuFlyoutPage()
	{
		InitializeComponent();
	}

    private void GoToHome(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("///HomePage");
        //Detail = new HomePage(); 
        IsPresented = true; 
    }

    private void GoToProfile(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("///ProfilePage");
        //Detail = new ProfilePage(); 
        IsPresented = true;
    }
}
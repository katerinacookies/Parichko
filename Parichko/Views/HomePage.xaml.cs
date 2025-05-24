using Parichko.ViewModels;

namespace Parichko.Views;

public partial class HomePage : ContentPage
{
    private readonly ExpenseViewModel _viewModel;
    public HomePage(ExpenseViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        Displaythename();
        System.Diagnostics.Debug.WriteLine("IncomeViewModel инициализиран.");
    }

    public async Task Displaythename()
	{
		//? await
		string name = Preferences.Get("LoggedUserName", "");
		displayhi.Text = "Здравей, " + name;
	}
}
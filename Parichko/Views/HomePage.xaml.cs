using Parichko.Utilities;
using Parichko.ViewModels;
using System.Globalization;

namespace Parichko.Views;

public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel _viewModel;
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Displaythename();
        SlayChart();
        //LoadExpenseInChart();
        System.Diagnostics.Debug.WriteLine("IncomeViewModel инициализиран.");
    }

    public async Task Displaythename()
	{
		//? await
		string name = Preferences.Get("LoggedUserName", "");
		displayhi.Text = "Здравей, " + name;
	}

    public async Task LoadExpenseInChart()
    {
        await _viewModel.LoadExpByDayAsync();
    }
    public async Task SlayChart()
    {
        _viewModel.LoadWeeklyExpenses();
    }
}
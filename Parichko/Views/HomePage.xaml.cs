using Parichko.Utilities;
using Parichko.ViewModels;
using System.Globalization;

namespace Parichko.Views;

public partial class HomePage : ContentPage
{
    private CancellationTokenSource _cts;
    private readonly HomePageViewModel _viewModel;
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        //LoadExpenseInChart();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _cts = new CancellationTokenSource();
        Displaythename();
        if (BindingContext is HomePageViewModel _viewModel)
        {
            await _viewModel.LoadWeeklyExpenses();
        }
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
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _cts.Cancel();
    }
}
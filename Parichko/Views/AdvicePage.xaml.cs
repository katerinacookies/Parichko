//using Microsoft.UI;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AdvicePage : ContentPage
{
    private readonly AdvicePageViewModel _viewModel;
    public AdvicePage(AdvicePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        LoadDoughnut();
        SlayChart();
        IncomeChart();
    }

    public async Task LoadDoughnut()
    {
        await _viewModel.LoadExpenseCategoryChart();
    }
    public async Task SlayChart()
    {
        _viewModel.LoadWeeklyExpenses();
    }
    public async Task IncomeChart()
    {
        _viewModel.LoadWeeklyIncomes();
    }
}
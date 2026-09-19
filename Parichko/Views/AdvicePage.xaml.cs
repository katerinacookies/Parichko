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
        EveryMonthChart();
    }

    public async Task LoadDoughnut()
    {
        await _viewModel.LoadExpenseCategoryChart();
    }
    public async Task SlayChart()
    {
        await _viewModel.LoadMonthlyExpenses();
    }
    public async Task EveryMonthChart()
    {
        await _viewModel.EveryMonthExpenses();
    }
    public async Task IncomeChart()
    {
        _viewModel.LoadWeeklyIncomes();
    }
}
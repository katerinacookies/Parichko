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
    }

    public async Task LoadDoughnut()
    {
        await _viewModel.LoadExpenseCategoryChart();
    }
}
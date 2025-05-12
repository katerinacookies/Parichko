using Parichko.ViewModels;

namespace Parichko.Views;

public partial class GoalsPage : ContentPage
{
    private readonly GoalViewModel _viewModel;
    public GoalsPage(GoalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        ShowGoals();
    }
    private async void ShowGoals()
    {
        await _viewModel.LoadGoalsAsync();
    }
}
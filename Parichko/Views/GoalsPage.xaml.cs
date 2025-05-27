using Parichko.Models;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class GoalsPage : ContentPage
{
    private readonly GoalViewModel _viewModel;
    private Goal chosenGoal;
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
    private async void OnAddProgressClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Goal goal)
        {
            chosenGoal = goal;
            decimal addedAmount = int.Parse(AddedAmountEntry.Text) ?? 0;
            await _viewModel.UpdateSavedAmountAsync(goal, addedAmount);
        }
    }
}
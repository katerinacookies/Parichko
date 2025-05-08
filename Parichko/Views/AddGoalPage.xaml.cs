using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AddGoalPage : ContentPage
{
    private readonly GoalViewModel _viewModel;
    public AddGoalPage(GoalViewModel viewModel)
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
    private async void OnAddClicked(object sender, EventArgs e)
    {
        string goalName = GoalnameEntry.Text.ToString();
        string goalColor = GoalColorEntry.Text.ToString();
        decimal goalAmount = decimal.Parse(GoalAmountEntry.Text);
        string goalIcon = IconnameEntry.Text.ToString();
        await _viewModel.AddGoalsAsync(goalName, goalAmount, goalIcon, goalColor);
    }
}
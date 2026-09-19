using Parichko.Models;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class GoalsPage : ContentPage
{
    private CancellationTokenSource _cts;
    private readonly GoalViewModel _viewModel;
    private Goal chosenGoal;
    public GoalsPage(GoalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        ShowGoals();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _cts = new CancellationTokenSource();
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

            // Find the parent of the button and then search for Entry
            var parent = button.Parent;
            Entry entry = null;

            while (parent != null && entry == null)
            {
                entry = VisualTreeHelper.FindDescendantByType<Entry>(parent);
                parent = parent.Parent;
            }

            if (entry != null && decimal.TryParse(entry.Text, out decimal addedAmount))
            {
                await _viewModel.UpdateSavedAmountAsync(goal, addedAmount);
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Не е въведена валидна сума.", "Добре");
                });
            }
            ShowGoals();
        }
    }
    private Entry FindEntryInVisualTree(Element startElement)
    {
        // Traverse up until we reach null or ContentView (your Frame or Grid likely)
        Element parent = startElement.Parent;

        while (parent != null)
        {
            if (parent is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    if (child is Entry entry)
                        return entry;

                    // Recursively search deeper if needed
                    if (child is Layout nestedLayout)
                    {
                        var result = FindEntryInLayout(nestedLayout);
                        if (result != null)
                            return result;
                    }
                }
            }

            parent = parent.Parent;
        }

        return null;
    }
    private Entry FindEntryInLayout(Layout layout)
    {
        foreach (var child in layout.Children)
        {
            if (child is Entry entry)
                return entry;

            if (child is Layout nestedLayout)
            {
                var result = FindEntryInLayout(nestedLayout);
                if (result != null)
                    return result;
            }
        }

        return null;
    }
    public static class VisualTreeHelper
    {
        public static T? FindDescendantByType<T>(Element element) where T : Element
        {
            if (element is T target)
                return target;

            if (element is IElementController controller)
            {
                foreach (var child in controller.LogicalChildren)
                {
                    var result = FindDescendantByType<T>(child);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _cts.Cancel();
    }
}
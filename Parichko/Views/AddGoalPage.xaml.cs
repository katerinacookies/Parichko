using Parichko.Models;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AddGoalPage : ContentPage
{
    private readonly GoalViewModel _viewModel;
    private string chosenIcon;
    private string chosenColor;
    private int addedFriend;
    public AddGoalPage(GoalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        //AddFriendDropdown.ItemsSource = _viewModel.Friends;
        ShowGoals();
    }
    private async void ShowGoals()
    {
        await _viewModel.LoadGoalsAsync();
    }
    private async void OnAddClicked(object sender, EventArgs e)
    {
        string goalName = GoalnameEntry.Text.ToString();
        decimal goalAmount = decimal.Parse(GoalAmountEntry.Text);
        await _viewModel.AddGoalsAsync(goalName, goalAmount, chosenIcon, chosenColor);
    }

    private void OpenPicker(object sender, EventArgs e)
    {
        GoalIcons.IsVisible = !GoalIcons.IsVisible;
    }
    public async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Goal goal)
        {
            int goalId = goal.Id;
            await _viewModel.DeleteGoal(goalId);
        }
    }
    private void OpenFriends(object sender, EventArgs e)
    {
        //AddFriendDropdown.IsVisible = !AddFriendDropdown.IsVisible;
    }
    private void OnFriendSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is UserProfile selectedFriend)
        {
            int chosen = selectedFriend.Id;
            DisplayAlert("Избран е приятел.", selectedFriend.DisplayName, "Добре");
            addedFriend = chosen;

            //AddFriendDropdown.IsVisible = false;
            //AddFriendDropdown.SelectedItem = null;
        }
    }
    public async void OnAddFriendClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Goal goal)
        {
            int goalId = goal.Id;
            //await _viewModel.AddFriendAsync(goalId, addedFriend);
        }
    }


    //Иконка на категория е натисната
    private void OnHealthClicked(object sender, EventArgs e)
    {
        chosenIcon = "healthgoal.png";
        chosenColor = "#f24a02";
        GoalIcons.IsVisible = false;
    }
    private void OnRetirementClicked(object sender, EventArgs e)
    {
        chosenIcon = "retirementgoal.png";
        chosenColor = "#0d522c";
        GoalIcons.IsVisible = false;
    }
    private void OnGroceriesClicked(object sender, EventArgs e)
    {
        chosenIcon = "groceriescat.png";
        chosenColor = "#561826";
        GoalIcons.IsVisible = false;
    }
    private void OnSofaClicked(object sender, EventArgs e)
    {
        chosenIcon = "sofagoal.png";
        chosenColor = "#004369";
        GoalIcons.IsVisible = false;
    }
    private void OnTravelClicked(object sender, EventArgs e)
    {
        chosenIcon = "travelgoal.png";
        chosenColor = "#004369";
        GoalIcons.IsVisible = false;
    }
    private void OnPresentsClicked(object sender, EventArgs e)
    {
        chosenIcon = "presentscat.png";
        chosenColor = "#ac8a02";
        GoalIcons.IsVisible = false;
    }
    private void OnMoneyClicked(object sender, EventArgs e)
    {
        chosenIcon = "moneygoal.png";
        chosenColor = "#265802";
        GoalIcons.IsVisible = false;
    }
    private void OnTicketsClicked(object sender, EventArgs e)
    {
        chosenIcon = "ticketscat.png";
        chosenColor = "#a7060f";
        GoalIcons.IsVisible = false;
    }
    private void OnHouseClicked(object sender, EventArgs e)
    {
        chosenIcon = "housegoal.png";
        chosenColor = "#0e522d";
        GoalIcons.IsVisible = false;
    }
    private void OnSchoolClicked(object sender, EventArgs e)
    {
        chosenIcon = "schoolcat.png";
        chosenColor = "#00448b";
        GoalIcons.IsVisible = false;
    }
    private void OnPetClicked(object sender, EventArgs e)
    {
        chosenIcon = "petcat.png";
        chosenColor = "#66200a";
        GoalIcons.IsVisible = false;
    }
    private void OnCharityClicked(object sender, EventArgs e)
    {
        chosenIcon = "charity.png";
        chosenColor = "#bb3fde";
        GoalIcons.IsVisible = false;
    }
    private void OnTechClicked(object sender, EventArgs e)
    {
        chosenIcon = "techcat.png";
        chosenColor = "#837500";
        GoalIcons.IsVisible = false;
    }
    private void OnTransportClicked(object sender, EventArgs e)
    {
        chosenIcon = "transportcat.png";
        chosenColor = "#d91a46";
        GoalIcons.IsVisible = false;
    }
    private void OnClothesClicked(object sender, EventArgs e)
    {
        chosenIcon = "clothescat.png";
        chosenColor = "#004263";
        GoalIcons.IsVisible = false;
    }
    private void OnBusinessClicked(object sender, EventArgs e)
    {
        chosenIcon = "businessgoal.png";
        chosenColor = "#f3b70f";
        GoalIcons.IsVisible = false;
    }
    private void OnBabyClicked(object sender, EventArgs e)
    {
        chosenIcon = "babycat.png";
        chosenColor = "#004263";
        GoalIcons.IsVisible = false;
    }
    private void OnWeddingClicked(object sender, EventArgs e)
    {
        chosenIcon = "weddinggoal.png";
        chosenColor = "#5a5858";
        GoalIcons.IsVisible = false;
    }
    private void OnGameClicked(object sender, EventArgs e)
    {
        chosenIcon = "gamegoal.png";
        chosenColor = "#e44201";
        GoalIcons.IsVisible = false;
    }
    private void OnOthersClicked(object sender, EventArgs e)
    {
        chosenIcon = "otherscat.png";
        chosenColor = "#5a5858";
        GoalIcons.IsVisible = false;
    }
}
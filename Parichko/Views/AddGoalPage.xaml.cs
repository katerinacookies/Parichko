using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AddGoalPage : ContentPage
{
    private readonly GoalViewModel _viewModel;
    private string chosenIcon;
    private string chosenColor;
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
        decimal goalAmount = decimal.Parse(GoalAmountEntry.Text);
        await _viewModel.AddGoalsAsync(goalName, goalAmount, chosenIcon, chosenColor);
    }

    private void OpenPicker(object sender, EventArgs e)
    {
        GoalIcons.IsVisible = !GoalIcons.IsVisible;
    }

    //Иконка на категория е натисната
    private void OnFoodClicked(object sender, EventArgs e)
    {
        chosenIcon = "foodcat.png";
        chosenColor = "#f24a02";
        GoalIcons.IsVisible = false;
    }
    private void OnTeaClicked(object sender, EventArgs e)
    {
        chosenIcon = "teacat.png";
        chosenColor = "#0d522c";
        GoalIcons.IsVisible = false;
    }
    private void OnGroceriesClicked(object sender, EventArgs e)
    {
        chosenIcon = "groceriescat.png";
        chosenColor = "#561826";
        GoalIcons.IsVisible = false;
    }
    private void OnCleanClicked(object sender, EventArgs e)
    {
        chosenIcon = "cleancat.png";
        chosenColor = "#004369";
        GoalIcons.IsVisible = false;
    }
    private void OnMedsClicked(object sender, EventArgs e)
    {
        chosenIcon = "medscat.png";
        chosenColor = "#004369";
        GoalIcons.IsVisible = false;
    }
    private void OnPresentsClicked(object sender, EventArgs e)
    {
        chosenIcon = "presentscat.png";
        chosenColor = "#ac8a02";
        GoalIcons.IsVisible = false;
    }
    private void OnNailsClicked(object sender, EventArgs e)
    {
        chosenIcon = "nailscat.png";
        chosenColor = "#265802";
        GoalIcons.IsVisible = false;
    }
    private void OnTicketsClicked(object sender, EventArgs e)
    {
        chosenIcon = "ticketscat.png";
        chosenColor = "#a7060f";
        GoalIcons.IsVisible = false;
    }
    private void OnBooksClicked(object sender, EventArgs e)
    {
        chosenIcon = "bookscat.png";
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
    private void OnSportsClicked(object sender, EventArgs e)
    {
        chosenIcon = "sportscat.png";
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
    private void OnWifiClicked(object sender, EventArgs e)
    {
        chosenIcon = "wificat.png";
        chosenColor = "#f3b70f";
        GoalIcons.IsVisible = false;
    }
    private void OnBabyClicked(object sender, EventArgs e)
    {
        chosenIcon = "babycat.png";
        chosenColor = "#004263";
        GoalIcons.IsVisible = false;
    }
    private void OnJewelsClicked(object sender, EventArgs e)
    {
        chosenIcon = "jewelscat.png";
        chosenColor = "#5a5858";
        GoalIcons.IsVisible = false;
    }
    private void OnUnderwearClicked(object sender, EventArgs e)
    {
        chosenIcon = "underwearcat.png";
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
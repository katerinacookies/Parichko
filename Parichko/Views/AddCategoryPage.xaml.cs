//using AndroidX.Lifecycle;

using Parichko.Models;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AddCategoryPage : ContentPage
{
	private readonly CategoryViewModel _viewModel;
    private string chosenIcon;
    private string chosenColor;
    private int deleteClicked = 0;
	public AddCategoryPage(CategoryViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
		_viewModel = viewModel;
        ShowCats();
    }
    private async void ShowCats()
    {
        await _viewModel.LoadCatsAsync();
    }
    private async void OnAddClicked(object sender, EventArgs e)
    {
        string catName = CatnameEntry.Text.ToString();
        await _viewModel.AddCategoryAsync(catName, chosenColor, chosenIcon);
    }
    private void OpenPicker(object sender, EventArgs e)
    {
        CategoryIcons.IsVisible = !CategoryIcons.IsVisible;
    }

    public async void OnDeleteClicked(object sender, EventArgs e)
    {
        deleteClicked++;
        if(deleteClicked == 1)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.DisplayAlert("Внимание!", "Всички разходи от тази категория също ще бъдат изтрити. Натиснете втори път за изтриване", "Разбрах");
            });
            
        }
        if(deleteClicked == 2)
        {
            if (sender is Button button && button.BindingContext is Category cat)
            {
                int catId = cat.Id;
                await _viewModel.DeleteCat(catId);
            }
        }
    }

    //Иконка на категория е натисната
    private void OnFoodClicked(object sender, EventArgs e)
    {
        chosenIcon = "foodcat.png";
        chosenColor = "#f24a02";
        CategoryIcons.IsVisible = false;
    }
    private void OnTeaClicked(object sender, EventArgs e)
    {
        chosenIcon = "teacat.png";
        chosenColor = "#0d522c";
        CategoryIcons.IsVisible = false;
    }
    private void OnGroceriesClicked(object sender, EventArgs e)
    {
        chosenIcon = "groceriescat.png";
        chosenColor = "#561826";
        CategoryIcons.IsVisible = false;
    }
    private void OnCleanClicked(object sender, EventArgs e)
    {
        chosenIcon = "cleancat.png";
        chosenColor = "#004369";
        CategoryIcons.IsVisible = false;
    }
    private void OnMedsClicked(object sender, EventArgs e)
    {
        chosenIcon = "medscat.png";
        chosenColor = "#004369";
        CategoryIcons.IsVisible = false;
    }
    private void OnPresentsClicked(object sender, EventArgs e)
    {
        chosenIcon = "presentscat.png";
        chosenColor = "#ac8a02";
        CategoryIcons.IsVisible = false;
    }
    private void OnNailsClicked(object sender, EventArgs e)
    {
        chosenIcon = "nailscat.png";
        chosenColor = "#265802";
        CategoryIcons.IsVisible = false;
    }
    private void OnTicketsClicked(object sender, EventArgs e)
    {
        chosenIcon = "ticketscat.png";
        chosenColor = "#a7060f";
        CategoryIcons.IsVisible = false;
    }
    private void OnBooksClicked(object sender, EventArgs e)
    {
        chosenIcon = "bookscat.png";
        chosenColor = "#0e522d";
        CategoryIcons.IsVisible = false;
    }
    private void OnSchoolClicked(object sender, EventArgs e)
    {
        chosenIcon = "schoolcat.png";
        chosenColor = "#00448b";
        CategoryIcons.IsVisible = false;
    }
    private void OnPetClicked(object sender, EventArgs e)
    {
        chosenIcon = "petcat.png";
        chosenColor = "#66200a";
        CategoryIcons.IsVisible = false;
    }
    private void OnSportsClicked(object sender, EventArgs e)
    {
        chosenIcon = "sportscat.png";
        chosenColor = "#bb3fde";
        CategoryIcons.IsVisible = false;
    }
    private void OnTechClicked(object sender, EventArgs e)
    {
        chosenIcon = "techcat.png";
        chosenColor = "#837500";
        CategoryIcons.IsVisible = false;
    }
    private void OnTransportClicked(object sender, EventArgs e)
    {
        chosenIcon = "transportcat.png";
        chosenColor = "#d91a46";
        CategoryIcons.IsVisible = false;
    }
    private void OnClothesClicked(object sender, EventArgs e)
    {
        chosenIcon = "clothescat.png";
        chosenColor = "#004263";
        CategoryIcons.IsVisible = false;
    }
    private void OnWifiClicked(object sender, EventArgs e)
    {
        chosenIcon = "wificat.png";
        chosenColor = "#f3b70f";
        CategoryIcons.IsVisible = false;
    }
    private void OnBabyClicked(object sender, EventArgs e)
    {
        chosenIcon = "babycat.png";
        chosenColor = "#004263";
        CategoryIcons.IsVisible = false;
    }
    private void OnJewelsClicked(object sender, EventArgs e)
    {
        chosenIcon = "jewelscat.png";
        chosenColor = "#5a5858";
        CategoryIcons.IsVisible = false;
    }
    private void OnUnderwearClicked(object sender, EventArgs e)
    {
        chosenIcon = "underwearcat.png";
        chosenColor = "#e44201";
        CategoryIcons.IsVisible = false;
    }
    private void OnOthersClicked(object sender, EventArgs e)
    {
        chosenIcon = "otherscat.png";
        chosenColor = "#5a5858";
        CategoryIcons.IsVisible = false;
    }
}
using Parichko.Models;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AddExpensePage : ContentPage
{
    private readonly ExpenseViewModel _viewModel;
    private readonly DropdownViewModel _viewModel1;
    private string chosenCat;
    public AddExpensePage(ExpenseViewModel viewModel, DropdownViewModel viewModel1)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        ShowExpenses();
        _viewModel1 = viewModel1;
        _viewModel.CategoriesForDropdown();
        categoryDropdown.ItemsSource = _viewModel.Categories;
        //PopulateDropdown();
    }
    private async void ShowExpenses()
    {
        await _viewModel.LoadExpensesAsync();
    }

    private void OpenPicker(object sender, EventArgs e)
    {
        categoryDropdown.IsVisible = !categoryDropdown.IsVisible;
    }
    private void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("SelectionChanged fired!");
        //var vm = BindingContext as DropdownViewModel;
        if (e.CurrentSelection.FirstOrDefault() is Category selectedItem)
        {
            chosenCat = selectedItem.Name.ToString();
            //chosenCat = Convert.ToString(categoryDropdown.SelectedItem);
            categoryDropdown.IsVisible = false;

            //
            categoryDropdown.SelectedItem = null;
        }
    }
    private void OnItemTapped(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Item was tapped");
    }
    private async void OnAddClicked(object sender, EventArgs e)
    {
        string expenseName = ExpensenameEntry.Text.ToString();
        decimal expenseAmount = decimal.Parse(ExpenseAmountEntry.Text);
        string expenseCat = chosenCat;
        //string expenseCat = CategoryDropdown.SelectedItem.ToString();
        await _viewModel.AddExpenseAsync(expenseName, expenseAmount, expenseCat);
    }

    

    /*private async void PopulateDropdown()
    {
        List<Category> categoriesFromDb = new List<Category>();
        await _viewModel.CategoriesForDropdown(categoriesFromDb);
        foreach(Category category in categoriesFromDb)
        {
            CategoryDropdown.Items.Add(category.Name);
        }
    }*/
}
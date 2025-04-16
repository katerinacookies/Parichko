using Parichko.Models;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AddExpensePage : ContentPage
{
    private readonly ExpenseViewModel _viewModel;
    public AddExpensePage(ExpenseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        ShowExpenses();

        //PopulateDropdown();
    }
    private async void ShowExpenses()
    {
        await _viewModel.LoadExpensesAsync();
    }

    private void OnShowClicked(object sender, EventArgs e)
    {
        //показва се
        categoryDropdown.IsVisible = !categoryDropdown.IsVisible;
    }

    private void OnCatSelected(object sender, EventArgs e)
    {

    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        string expenseName = ExpensenameEntry.Text.ToString();
        decimal expenseAmount = decimal.Parse(ExpenseAmountEntry.Text);
        //string expenseCat = CategoryDropdown.SelectedItem.ToString();
        //await _viewModel.AddExpenseAsync(expenseName, expenseAmount, expenseCat);
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
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
        //var vm = BindingContext as DropdownViewModel;
        if (e.CurrentSelection.FirstOrDefault() is Category selectedItem)
        {
            string chosen = selectedItem.Name;
            DisplayAlert("Избрана е категория.", chosen, "Добре");
            chosenCat = chosen;

            categoryDropdown.IsVisible = false;

            //
            categoryDropdown.SelectedItem = null;


            //vm.SelectedCategory = selected;
            //categoryDropdown.IsVisible = false;
        }
    }
    private async void OnAddClicked(object sender, EventArgs e)
    {
        string expenseName = ExpensenameEntry.Text.ToString();
        decimal expenseAmount = decimal.Parse(ExpenseAmountEntry.Text);
        string expenseCat = chosenCat;
        //string expenseCat = CategoryDropdown.SelectedItem.ToString();
        await _viewModel.AddExpenseAsync(expenseName, expenseAmount, expenseCat);
    }

    public async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Expense expense)
        {
            int expenseId = expense.Id;
            await _viewModel.DeleteExpense(expenseId);
        }
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
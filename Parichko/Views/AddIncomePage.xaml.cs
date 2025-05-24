using Parichko.Models;
using Parichko.ViewModels;

namespace Parichko.Views;


public partial class AddIncomePage : ContentPage
{
	private readonly IncomeViewModel _viewModel;
	public AddIncomePage(IncomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
		ShowIncomes();
	}

    private async void ShowIncomes()
    {
        await _viewModel.LoadIncomesAsync();
    }
    private async void OnAddClicked(object sender, EventArgs e)
    {
        string incomeName = IncomenameEntry.Text.ToString();
        decimal incomeAmount = decimal.Parse(IncomeAmountEntry.Text);
        await _viewModel.AddIncomesAsync(incomeName, incomeAmount);
    }
    public async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Income income)
        {
            int incomeId = income.Id;
            await _viewModel.DeleteIncome(incomeId);
        }
    }
}
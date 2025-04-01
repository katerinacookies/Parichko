//using AndroidX.Lifecycle;

using Parichko.ViewModels;

namespace Parichko.Views;

public partial class AddCategoryPage : ContentPage
{
	private readonly CategoryViewModel _viewModel;
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
        string catColor = ColorEntry.Text.ToString();
        string catIcon = IconEntry.Text.ToString();
        await _viewModel.AddCategoryAsync(catName, catColor, catIcon);
    }
}
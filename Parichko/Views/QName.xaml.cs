using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Parichko.Data;
using Parichko.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Parichko.ViewModels;
namespace Parichko.Views;

public partial class QName : ContentPage
{
    private readonly QNameViewModel _viewModel;
    public QName(QNameViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }
    

    private async void OnForwardBtnClicked(object sender, EventArgs e)
	{
        try
        {
            string displayname = UsernameEntry.Text.ToString();
            await _viewModel.SetDisplayNameAsync(displayname);
            await Shell.Current.GoToAsync("///AllDonePage?refresh=true");
        }
		catch(Exception ex)
        {
            await DisplayAlert("Грешка", ex.Message, "Добре");
        }
		
	}
}
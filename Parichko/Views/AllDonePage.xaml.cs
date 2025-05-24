using Syncfusion.Maui.Core.Carousel;

namespace Parichko.Views;

public partial class AllDonePage : ContentPage
{
	public AllDonePage()
	{
		InitializeComponent();
	}
    private async void OnForwardBtnClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("///HomePage?refresh=true");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Грешка", ex.Message, "Добре");
        }

    }
}
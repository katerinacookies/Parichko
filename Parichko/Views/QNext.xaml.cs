namespace Parichko.Views;

public partial class QNext : ContentPage
{
    public QNext()
    {
        InitializeComponent();
    }

    private async void OnGotitClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///QName");
    }
}
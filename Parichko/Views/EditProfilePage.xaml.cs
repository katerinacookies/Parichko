using Parichko.ViewModels;
using Syncfusion.Maui.Core.Carousel;

namespace Parichko.Views;

public partial class EditProfilePage : ContentPage
{
    private readonly EditPageViewModel _viewModel;
    public EditProfilePage(EditPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        DisplayData();
    }

    public async Task DisplayData()
    {
        Dictionary<string, string> data = await _viewModel.LoadProfileDataAsync();
        DisplayName.Placeholder = data["Name"];
        UserEmail.Placeholder = data["Email"];
        UserPass.Placeholder = data["Pass"];
    }
    public async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ProfilePage");
    }
    public async void OnSaveClicked(object sender, EventArgs e)
    {
        string userName = DisplayName.Text ?? String.Empty;
        string userEmail = (UserEmail.Text ?? String.Empty).ToLower();
        string userPass = UserPass.Text ?? String.Empty;
        string userPassRepeat = UserPassRepeat.Text ?? String.Empty;

        await _viewModel.UpdateProfileDataAsync(userName, userEmail, userPass, userPassRepeat);
    }
}
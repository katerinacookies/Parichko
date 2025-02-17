using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Parichko.Data;
using Parichko.Models;
namespace Parichko.Views;

public partial class LoginPage : ContentPage
{
    private readonly ParichkoDbContext _context;
	public LoginPage()
	{
		InitializeComponent();
        _context = new ParichkoDbContext();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string userEmail = EmailEntry.Text.ToLower().ToString();
        string userPass = PassEntry.Text.ToLower().ToString();

        //var user = await _context.FindAsync<Login>(userEmail);
        Login login = _context.Logins.Where(l => l.Email == userEmail && l.PasswordHash == userPass).FirstOrDefault();

        if(login != null)
        {
            Preferences.Set("LoggedUserId", login.UserProfile.Id);

            await Shell.Current.GoToAsync("///HomePage");

            await DisplayAlert("Супер <3", "Влязохте успешно!", "Добре");
        }
        else
        {
            await DisplayAlert("Възникна проблем", "Грешен имейл или парола.", "Добре");
        }
    }
}
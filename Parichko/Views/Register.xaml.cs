using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Parichko.Data;
using Parichko.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Parichko.ViewModels;

namespace Parichko.Views;

public partial class Register : ContentPage
{
	private readonly RegisterViewModel _viewModel;
	public Register(RegisterViewModel viewModel)
	{
		InitializeComponent();
        //_context = new ParichkoDbContext();
        _viewModel = viewModel;
        //_context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        //await DisplayAlert("Debug", "Button Clicked!", "OK");

        //await Shell.Current.GoToAsync("///HomePage");

        try
        {
            string userEmail = EmailEntry.Text.ToLower();
            string userPass = PassEntry.Text.ToString();
            string userPass2 = PassRepeat.Text.ToString();

            await _viewModel.RegisterAsync(userEmail, userPass, userPass2);

            await Shell.Current.GoToAsync("///HomePage");
            
            /*if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPass) || string.IsNullOrWhiteSpace(userPass2))
            {
                await DisplayAlert("Възникна грешка", "Полетата за попълване са задължителни!", "Добре");
                return;
            }

            if (userPass != userPass2)
            {
                await DisplayAlert("Възникна грешка", "Паролите трябва да съвпадат!", "Добре");
                return;
            }

            //await DisplayAlert("Слей", "Полетата не са нъл", "Добре");

            if (_context == null)
            {
                await DisplayAlert("Възникна грешка", "Контекстът е нъл :(", "Добре");
            }

            //Show Loading Indicator (Disables UI While Processing)
            LoginBtn.IsEnabled = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            Login userFromDbFunc = await Task.Run(async () =>
            {
                Login userFromDb = await _context.Logins
                        .AsNoTracking() //Оптимизира куерито
                        .FirstOrDefaultAsync(l => l.Email == userEmail);

                return userFromDb;
            });

            if (userFromDbFunc != null)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Възникна грешка", "Потребител с такъв имейл вече съществува.", "Добре");
                });
                return;
            }
            else 
            {
                await DisplayAlert("Възникна грешка", "mai e ok.", "Добре");
            }*/


        /*bool isSuccess = await Task.Run(async () =>
        {
            try
            {
                Login userFromDb = await _context.Logins
                    .AsNoTracking() //Оптимизира куерито
                    .FirstOrDefaultAsync(l => l.Email == userEmail);

                if (userFromDb != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Възникна грешка", "Потребител с такъв имейл вече съществува.", "Добре");
                    });
                    return false;
                }

                Login newLogin = new Login()
                {
                    Email = userEmail,
                    PasswordHash = userPass
                };
                UserProfile newProfile = new UserProfile()
                {
                    Login = newLogin
                };
                newLogin.UserProfile = newProfile;

                //kak da polzvam CreatAsync tuk?
                await _context.Logins.AddAsync(newLogin);
                await _context.UserProfiles.AddAsync(newProfile);
                await _context.SaveChangesAsync();

                Preferences.Set("LoggedUserId", newProfile.Id);

                return true;
                //UI навигация и алерти  трябва да са в основната нишка

            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Error", $"DB Error: {ex.Message}", "OK");
                });
                return false;
            }
        });



        // Hide Loading Indicator & Enable Button Again
        LoginBtn.IsEnabled = true;
        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;

        if (isSuccess)
        {
            await Shell.Current.GoToAsync("///HomePage");
        }*/
    }
    catch(Exception ex)
    {
        await DisplayAlert("Error", $"Unexpected Error: {ex.Message}", "OK");
    }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Parichko.Data;
using Parichko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public partial class LoginViewModel
    {
        private readonly ParichkoDbContext _context;

        public LoginViewModel(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(string userEmail, string userPass)
        {
            try
            {
                if (_context == null)
                {
                    await Shell.Current.DisplayAlert("shi", "null context", "ok");
                    return false;
                }
                userEmail = userEmail?.Trim().ToLower();
                userPass = userPass?.Trim();

                if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPass))
                {
                    await Shell.Current.DisplayAlert("Грешка", "Попълнете всички полета!", "Добре");
                    return false;
                }


                var userFromDb = await Task.Run(async () =>
                    _context.Logins.FirstOrDefault(l => l.Email == userEmail && l.PasswordHash == userPass));

                if (userFromDb != null)
                {
                    await Shell.Current.DisplayAlert("Грешка", "Този потребител вече съществува!", "Добре");
                    return false;
                }

                await Task.Run(async () =>
                {
                    var newLogin = new Login
                    {
                        Email = userEmail,
                        PasswordHash = userPass
                    };
                    var newProfile = new UserProfile
                    {
                        Login = newLogin
                    };

                    newLogin.UserProfile = newProfile;

                    await _context.Logins.AddAsync(newLogin);
                    await _context.UserProfiles.AddAsync(newProfile);
                    await _context.SaveChangesAsync();

                    Preferences.Set("LoggedUserId", newProfile.Id);
                });

                await Shell.Current.GoToAsync("///HomePage");
                return true;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                return false;
            }
        }
    }
}

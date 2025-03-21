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

        public async Task<bool> LoginAsync(string? userEmail, string? userPass)
        {
            try
            {
                if (_context == null)
                {
                    await Shell.Current.DisplayAlert("Грешка", "Нъл контекст", "Добре");
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
                

                if (userFromDb == null)
                {
                    await Shell.Current.DisplayAlert("Грешка", "Няма такъв потребител!", "Добре");
                    return false;
                }

                var userprofileFromDb = await Task.Run(async () =>
                    _context.UserProfiles.FirstOrDefault(u => u.LoginId == userFromDb.Id));

                if (userprofileFromDb == null)
                {
                    await Shell.Current.DisplayAlert("Грешка", "Няма такъв потребителски профил!", "Добре");
                    return false;
                }

                await Task.Run(async () =>
                {
                    Preferences.Set("LoggedUserId", userprofileFromDb.Id);
                    Preferences.Set("LoggedUserName", userprofileFromDb.DisplayName);
                    Preferences.Set("LoggedUserEmail", userEmail);
                    Preferences.Set("LoggedUserPic", userprofileFromDb.ProfilePic);
                });

                if(userEmail == "admin@admin.com")
                {
                    await Shell.Current.GoToAsync("///AdminHomePage?refresh=true");
                    return true;
                }

                await Shell.Current.GoToAsync("///HomePage?refresh=true");
                return true;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                if(ex.InnerException != null)
                {
                    await Shell.Current.DisplayAlert("Грешка", ex.InnerException.Message, "Добре");
                }
                return false;
            }
        }
    }
}

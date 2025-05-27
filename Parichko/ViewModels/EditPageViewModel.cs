using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class EditPageViewModel
    {
        private readonly ParichkoDbContext _context;

        public EditPageViewModel(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> LoadProfileDataAsync()
        {
            Dictionary<string, string> userData = new Dictionary<string, string>();
            try
            {
                int userId = Preferences.Get("LoggedUserId", 0);
                var currentUser = await _context.UserProfiles
                    .Include(up => up.Login)
                    .FirstOrDefaultAsync(up => up.Id == userId);
                if(currentUser == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Потребителят не е намерен.", "Добре");
                    });
                    return userData;
                }
                userData["Name"] = currentUser.DisplayName;
                userData["Email"] = currentUser.Login.Email;
                userData["Password"] = currentUser.Login.PasswordHash;
                return userData;
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                });
                return userData;
            }
        }

        public async Task<bool> UpdateProfileDataAsync(string userName, string userEmail, string userPass, string userPassRepeat)
        {
            try
            {
                int userId = Preferences.Get("LoggedUserId", 0);
                var currentUser = await _context.UserProfiles
                    .Include(up => up.Login)
                    .FirstOrDefaultAsync(up => up.Id == userId);
                if(currentUser == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Потребителят не е октрит.", "Добре");
                    });
                    return false;
                }
                if(userName == "" && userEmail == "" && userPass == "")
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Не са въведени промени.", "Добре");
                    });
                    return false;
                }
                if(userName != "")
                {
                    userName = userName.Trim();
                    currentUser.DisplayName = userName;
                    Preferences.Set("LoggedUserName", userName);
                }
                if (userEmail != "")
                {
                    userEmail = userEmail.ToLower().Trim();
                    currentUser.Login.Email = userEmail;
                }
                if (userPass != "")
                {
                    if(userPassRepeat != "")
                    {
                        if(userPassRepeat == userPass)
                        {
                            currentUser.Login.PasswordHash = userPass;
                        }
                        else
                        {
                            await MainThread.InvokeOnMainThreadAsync(() =>
                            {
                                Shell.Current.DisplayAlert("Грешка", "Паролите не съвпадат.", "Добре");
                            });
                            return false;
                        }
                    }
                    else
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Shell.Current.DisplayAlert("Грешка", "Повторете паролата.", "Добре");
                        });
                        return false;
                    }
                }

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Готово", "Данните са променени успешно!", "Добре");
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                });
                return false;
            }
        }
    }
}

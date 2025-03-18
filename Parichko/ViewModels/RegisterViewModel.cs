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
    public partial class RegisterViewModel
    {
        private readonly ParichkoDbContext _context;

        public RegisterViewModel(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(string? userEmail, string? userPass, string? userPass2)
        {
            try
            {
                if (_context == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("shi", "null context", "ok");
                    });
                    return false;
                }
                userEmail = userEmail?.Trim().ToLower();
                userPass = userPass?.Trim();
                userPass2 = userPass2?.Trim();

                if(string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPass) || string.IsNullOrWhiteSpace(userPass2))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Попълнете всички полета!", "Добре");
                    });
                    return false;
                }

                if(userPass != userPass2)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Паролите не съвпадат :(", "Дорбе");
                    });
                    return false;
                }

                
                var userFromDb = await Task.Run(async () =>
                    _context.Logins.FirstOrDefault(l => l.Email == userEmail && l.PasswordHash == userPass));

                if(userFromDb != null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Този потребител вече съществува!", "Добре");
                    });
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
                        Login = newLogin,
                        DisplayName = "Proba"
                    };

                    newLogin.UserProfile = newProfile;

                    await _context.Logins.AddAsync(newLogin);
                    await _context.UserProfiles.AddAsync(newProfile);
                    Preferences.Set("LoggedUserId", newProfile.Id);
                    

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                        });
                        if (ex.InnerException != null)
                        {
                            await MainThread.InvokeOnMainThreadAsync(() =>
                            {
                                Shell.Current.DisplayAlert("Грешка", ex.InnerException.Message, "Добре");
                            });
                            if(ex.InnerException.InnerException != null)
                            {
                                await MainThread.InvokeOnMainThreadAsync(() =>
                                {
                                    Shell.Current.DisplayAlert("Грешка", ex.InnerException.InnerException.Message, "Добре");
                                });
                                if (ex.InnerException.InnerException.InnerException != null)
                                {
                                    await MainThread.InvokeOnMainThreadAsync(() =>
                                    {
                                        Shell.Current.DisplayAlert("Грешка", ex.InnerException.InnerException.InnerException.Message, "Добре");
                                    });
                                }
                            }
                        }
                    }

                    Preferences.Set("LoggedUserId", newProfile.Id);
                });

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.GoToAsync("///QNext");
                });
                return true;
            }
            catch(Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                });

                if(ex.InnerException != null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", ex.InnerException.Message, "Добре");
                    });
                    if(ex.InnerException.InnerException != null)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Shell.Current.DisplayAlert("Грешка", ex.InnerException.InnerException.Message, "Добре");
                        });
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            await MainThread.InvokeOnMainThreadAsync(() =>
                            {
                                Shell.Current.DisplayAlert("Грешка", ex.InnerException.InnerException.InnerException.Message, "Добре");
                            });
                        }
                    }
                }
                return false;
            }
        }
    }
}

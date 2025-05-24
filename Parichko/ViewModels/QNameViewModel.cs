using Parichko.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class QNameViewModel
    {
        private readonly ParichkoDbContext _context;

        public QNameViewModel(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SetDisplayNameAsync(string displayname)
        {
            try
            {
                if (_context == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Нъл контекст", "Добре");
                    });
                    return false;
                }

                displayname = displayname.Trim();
                //Присвояване на ID на влезлия потребител
                int userId = Preferences.Get("LoggedUserId", 0);
                if (string.IsNullOrWhiteSpace(displayname))
                {
                    await Shell.Current.DisplayAlert("Грешка", "Попълнете полето!", "Добре");
                    return false;
                }

                var userFromDb = await Task.Run(async () =>
                    _context.UserProfiles.FirstOrDefault(u => u.Id == userId));

                if (userFromDb == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Този потребител не съществува!", "Добре");
                    });
                    return false;
                }

                userFromDb.DisplayName = displayname;
                Preferences.Set("LoggedUserName", userFromDb.DisplayName);
                
                _context.UserProfiles.Update(userFromDb);

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
                        if (ex.InnerException.InnerException != null)
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
                return true;
            }
            catch(Exception ex)
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
                    if (ex.InnerException.InnerException != null)
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

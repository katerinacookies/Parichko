using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class CategoryViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<Category> Categories { get; set; } = new();

        public CategoryViewModel(ParichkoDbContext context)
        {
            _context = context;
            //LoadCatsAsync();
        }
        public async Task<bool> LoadCatsAsync()
        {
            try
            {
                int userId = Preferences.Get("LoggedUserId", 0);
                var catsFromDb = _context.Categories
                    .Where(c => c.UserProfileId == userId)
                    .ToList();

                Categories.Clear();

                foreach (var cat in catsFromDb)
                {
                    Categories.Add(cat);
                }
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

        public async Task<bool> AddCategoryAsync(string name, string? color, string iconName)
        {
            try
            {
                if (_context == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Контекстът е нъл.", "Добре");
                    });
                    return false;
                }
                name = name.Trim().ToString();

                if (string.IsNullOrWhiteSpace(name))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Попълнете всички полета!", "Добре");
                    });
                    return false;
                }

                var catFromDb = await Task.Run(async () =>
                    _context.Categories.FirstOrDefault(c => c.Name == name));

                if (catFromDb != null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Тази категория вече съществува!", "Добре");
                    });
                    return false;
                }

                await Task.Run(async () =>
                {
                    int userId = Preferences.Get("LoggedUserId", 0);
                    var currentUser = await _context.UserProfiles
                        .FirstOrDefaultAsync(up => up.Id == userId);
                    if(currentUser == null)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Shell.Current.DisplayAlert("Грешка", "Текущият потребител не е открит.", "Добре");
                        });
                        return false;
                    }
                    var newCat = new Category
                    {
                        Name = name,
                        Color = color,
                        IconName = iconName,
                        Expenses = new List<Expense>(),
                        UserProfileId = userId,
                        UserProfile = currentUser
                    };
                    currentUser.Categories.Add(newCat);
                    await _context.Categories.AddAsync(newCat);

                    try
                    {
                        await _context.SaveChangesAsync();
                        //ЗА ИЗПЪЛНЕНИЕ:
                        //проверка за категорията
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Shell.Current.DisplayAlert("Готово", name, newCat.Id.ToString());
                        });

                        Categories.Add(newCat);
                        return true;
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
                        return false;
                    }
                });
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

        public async Task<bool> DeleteCat(int catId)
        { 
            var currentCat = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == catId);

            if (currentCat == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Няма такава категория.", "Добре");
                });
                return false;
            }

            int userId = Preferences.Get("LoggedUserId", 0);
            var currentUser = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.Id == userId);
            if (currentUser == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Текущият потребител не е намерен.", "Добре");
                });
                return false;
            }
            currentUser.Categories.Remove(currentCat);
            try
            {
                
                var expenses = await _context.Expenses
                    .Where(e => e.CategoryId == catId)
                    .ToListAsync();

                foreach (var expense in expenses)
                {
                    _context.Expenses.Remove(expense);
                }

                Categories.Remove(currentCat);

                _context.Categories.Remove(currentCat);

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

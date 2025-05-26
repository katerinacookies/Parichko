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
    public class ExpenseViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<Expense> Expenses { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Expense> ExpensesForChart { get; set; }
        public ExpenseViewModel()
        {

        }
        public ExpenseViewModel(ParichkoDbContext context)
        {
            _context = context;
        }
        public async Task<bool> LoadExpensesAsync()
        {
            try
            {
                var expensesFromDb = _context.Expenses.Include(e => e.Category).ToList();

                Expenses.Clear();
                //Categories.Clear();

                foreach (var expense in expensesFromDb)
                {
                    Expenses.Add(expense);
                    //Categories.Add(expense.Category);
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

        public async Task<bool> AddExpenseAsync(string? name, decimal amount, string categoryId)
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

                if (string.IsNullOrWhiteSpace(name) || amount == 0)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Попълнете всички полета!", "Добре");
                    });
                    return false;
                }

                //Проверка за категорията
               
                var categoryFromDb = await Task.Run(async () =>
                    _context.Categories.FirstOrDefault(c => c.Name == categoryId));
               
                if(categoryFromDb == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Няма такава категория.", "Добре");
                    });
                    return false;
                }
                

                await Task.Run(async () =>
                {
                    int currentUserId = Preferences.Get("LoggedUserId", 0);

                    var currentUser = await _context.UserProfiles
                        .FirstOrDefaultAsync(up => up.Id == currentUserId);

                    if(currentUser == null)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Shell.Current.DisplayAlert("Грешка", "Текущият потребител не може да бъде намерен.", "Добре");
                        });
                        return false;
                    }
                    var newExpense = new Expense();

                    newExpense.Name = name;
                    newExpense.Amount = amount;
                    newExpense.CategoryId = categoryFromDb.Id;
                    newExpense.Category = categoryFromDb;
                    newExpense.UserProfile = currentUser;
                    newExpense.UserProfileId = currentUserId;
                   

                    await _context.Expenses.AddAsync(newExpense);

                    try
                    {
                        categoryFromDb.Expenses.Add(newExpense);
                        await _context.SaveChangesAsync();

                        Expenses.Add(newExpense);
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
        }

        public async Task<bool> CategoriesForDropdown()
        {
            try
            {
                var catsFromDb = await _context.Categories.ToListAsync();
                Categories.Clear();
                foreach(Category cat in catsFromDb)
                {
                    Categories.Add(cat);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteExpense(int expenseId)
        {
            var currentExpense = await _context.Expenses
                    .FirstOrDefaultAsync(e => e.Id == expenseId);

            if (currentExpense == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Няма такъв разход.", "Добре");
                });
                return false;
            }

            try
            {
                Expenses.Remove(currentExpense);

                _context.Expenses.Remove(currentExpense);

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

        private async void LoadWeeklyChart()
        {
            var allExpenses = Expenses.ToList();

            var startOfWeek = GetStartOfWeek();
            var endOfWeek = startOfWeek.AddDays(7);

            var weeklyExpenses = allExpenses
                .Where(e => e.Date >= startOfWeek && e.Date < endOfWeek)
                .ToList();

            var grouped = allExpenses
                .Where(e => e.Date >= startOfWeek && e.Date < endOfWeek)
                .GroupBy(e => e.Date.DayOfWeek)
                .OrderBy(g => g.Key)
                .Select(g => new Expense
                {
                    Date = DateTimeOffset.Now, // just a placeholder
                    Amount = g.Sum(x => x.Amount),
                    // Use Id to store day of week order, optional
                    Id = (int)g.Key
                });

            // Use Display-friendly version of the day as the tag
            ExpensesForChart = new ObservableCollection<Expense>(
                grouped.Select(e =>
                {
                    //e.Name = TranslateDay((DayOfWeek)e.Id); // You'd have to add a Name prop to Expense
                    return e;
                }));

            //OnPropertyChanged(nameof(ExpensesForChart));
        }

        private DateTimeOffset GetStartOfWeek()
        {
            var now = DateTimeOffset.Now;
            int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startOfWeek = now.AddDays(-diff);
            return startOfWeek.Date; // strips time, keeps date only
        }
    }
}

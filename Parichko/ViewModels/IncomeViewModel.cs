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
    public class IncomeViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<Income> Incomes { get; set; } = new();
        public List<Income> Data { get; set; }

        public IncomeViewModel(ParichkoDbContext context)
        {
            _context = context;

            Data = new List<Income>();
            Data = Incomes.ToList();
        }
        public IncomeViewModel()
        {

        }
        public async Task<bool> LoadIncomesAsync()
        {
            try
            {
                var incomesFromDb = await _context.Incomes
                    .Where(i => i.UserProfileId == Preferences.Get("LoggedUserId", 0))
                    .ToListAsync();

                Incomes.Clear();

                foreach (var income in incomesFromDb)
                {
                    Incomes.Add(income);
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

        public async Task<bool> AddIncomesAsync(string name, decimal amount)
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


                await Task.Run(async () =>
                {
                    Income newIncome = new Income();
                    newIncome.Name = name;
                    newIncome.Amount = amount;
                    newIncome.UserProfileId = Preferences.Get("LoggedUserId", 0);

                    var currentUser = await _context.UserProfiles
                        .FirstOrDefaultAsync(up => up.Id == Preferences.Get("LoggedUserId", 0));

                    _context.Incomes.Add(newIncome);
                    currentUser.Incomes.Add(newIncome);

                    try
                    {
                        _context.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Приходът е добавен!");
                        Incomes.Add(newIncome);
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
        public async Task<bool> DeleteIncome(int incomeId)
        {
            var currentIncome = await _context.Incomes
                    .FirstOrDefaultAsync(i => i.Id == incomeId);

            if (currentIncome == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Няма такъв приход.", "Добре");
                });
                return false;
            }

            try
            {
                Incomes.Remove(currentIncome);

                _context.Incomes.Remove(currentIncome);

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

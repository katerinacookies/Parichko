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
                    /*var newExpense = new Expense
                    {
                        Name = "Giros",
                        Amount = 3.40M,
                        CategoryId = 2,
                        Category = categoryFromDb
                    };*/
                    var newExpense = new Expense
                    {
                        Name = name,
                        Amount = amount,
                        CategoryId = categoryFromDb.Id,
                        Category = categoryFromDb
                    };

                    await _context.Expenses.AddAsync(newExpense);

                    try
                    {
                        await _context.SaveChangesAsync();
                        categoryFromDb.Expenses.Add(newExpense);

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
    }
}

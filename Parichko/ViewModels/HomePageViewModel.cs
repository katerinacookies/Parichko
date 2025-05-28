using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Models;
using Parichko.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Windows.System;
//using Windows.System;

namespace Parichko.ViewModels
{
    public class HomePageViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<ExpenseDay> DayChartExpenses { get; set; } = new();
        public ObservableCollection<Chart> WeeklyExpenseChartData { get; set; } = new();

        public HomePageViewModel(ParichkoDbContext context)
        {
            _context = context;
        }
        public async void LoadWeeklyExpenses()
        {
            WeeklyExpenseChartData.Clear();

            var today = DateTimeOffset.Now.Date;
            var last7Days = Enumerable.Range(0, 7)
                                      .Select(i => today.AddDays(-i))
                                      .OrderBy(d => d)
                                      .ToList();

            int userId = Preferences.Get("LoggedUserId", 0);
            var allExpenses = await _context.Expenses
                .Where(e => e.UserProfileId == userId)
                .ToListAsync();
            string day = String.Empty;
            string dayBG = String.Empty;
            foreach (var date in last7Days)
            {
                //decimal total = 20;
                decimal total = allExpenses
                    .Where(e => e.Date.DayOfWeek == date.DayOfWeek)
                    .Sum(e => e.Amount);
                day = date.ToString("ddd", CultureInfo.InvariantCulture);
                switch (day)
                {
                    case "Mon":
                        dayBG = "Пн";
                        break;
                    case "Tue":
                        dayBG = "Вт";
                        break;
                    case "Wed":
                        dayBG = "Ср";
                        break;
                    case "Thu":
                        dayBG = "Чт";
                        break;
                    case "Fri":
                        dayBG = "Пт";
                        break;
                    case "Sat":
                        dayBG = "Сб";
                        break;
                    case "Sun":
                        dayBG = "Нд";
                        break;
                }
                WeeklyExpenseChartData.Add(new Chart
                {
                    Day = dayBG,
                    Amount = total
                });
            }
        }
        public async Task<bool> LoadExpByDayAsync()
        {
            try
            {
                DateTimeOffset now = DateTimeOffset.Now;
                DateTimeOffset endOfWeek = now;
                DateTimeOffset startOfWeek = now.Subtract(TimeSpan.FromDays(7));
                
                var expenses = await _context.Expenses
                    .ToListAsync();

                expenses = expenses.OrderByDescending(e => e.Date).ToList();

                DayChartExpenses.Clear();

                DateTimeOffset currentDayOfWeek = startOfWeek;
                string day = String.Empty;
                for (int i = 0; i < 7; i++)
                {
                    ExpenseDay expD = new ExpenseDay();

                    decimal sum = 0;
                    foreach (Expense expense in expenses)
                    {
                        DateTimeOffset expenseDay = expense.Date;
                        if (expenseDay == currentDayOfWeek)
                        {
                            decimal amount = expense.Amount;
                            sum += amount;
                        }
                    }
                    expD.Amount = sum;

                    switch (currentDayOfWeek.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            expD.Day = "Пн";
                            break;
                        case DayOfWeek.Tuesday:
                            expD.Day = "Вт";
                            break;
                        case DayOfWeek.Wednesday:
                            expD.Day = "Ср";
                            break;
                        case DayOfWeek.Thursday:
                            expD.Day = "Чт";
                            break;
                        case DayOfWeek.Friday:
                            expD.Day = "Пт";
                            break;
                        case DayOfWeek.Saturday:
                            expD.Day = "Сб";
                            break;
                        case DayOfWeek.Sunday:
                            expD.Day = "Нд";
                            break;
                    }
                    DayChartExpenses.Add(expD);
                    currentDayOfWeek.AddDays(1);
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
        public async Task<bool> LoadExpensesAsync()
        {
            try
            {
                DateTimeOffset now = DateTimeOffset.Now;
                DateTimeOffset endOfWeek = now;
                DateTimeOffset startOfWeek = now.Subtract(TimeSpan.FromDays(7));

                var expenses = await _context.Expenses
                    .ToListAsync();

                expenses.OrderByDescending(e => e.Date.UtcDateTime);

                DayChartExpenses.Clear();

                List<decimal> expByDay = new List<decimal>();

                DateTimeOffset currentDayOfWeek = startOfWeek;
                string day = String.Empty;
                for (int i = 0; i < 7; i++)
                {
                    decimal sum = 0;
                    foreach(Expense expense in expenses)
                    {
                        if(expense.Date == currentDayOfWeek)
                        {
                            sum += expense.Amount;
                        }
                    }
                    expByDay.Add(sum);
                    switch(currentDayOfWeek.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            day = "Пн";
                            break;
                        case DayOfWeek.Tuesday:
                            day = "Вт";
                            break;
                        case DayOfWeek.Wednesday:
                            day = "Ср";
                            break;
                        case DayOfWeek.Thursday:
                            day = "Чт";
                            break;
                        case DayOfWeek.Friday:
                            day = "Пт";
                            break;
                        case DayOfWeek.Saturday:
                            day = "Сб";
                            break;
                        case DayOfWeek.Sunday:
                            day = "Нд";
                            break;
                    }
                    //UserChartExpenses[day] = sum;   
                    //currentDayOfWeek.
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
        public async Task<List<ExpenseCategory>> LoadExpByCatAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Expenses)
                .ToListAsync();

            var expenses = await _context.Expenses
                .ToListAsync();

            var data = categories.Select(cat => new ExpenseCategory
            {
                CategoryName = cat.Name,
                TotalExpense = expenses
                    .Where(exp => exp.CategoryId == cat.Id)
                    .Sum(exp => exp.Amount)
            }).ToList();

            return data;
        }
    }
}

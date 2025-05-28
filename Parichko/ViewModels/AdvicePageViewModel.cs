using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class AdvicePageViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<Brush> CustomBrushes { get; set; }
        public ObservableCollection<ExpenseCategory> CategoryExpenseData { get; set; } = new();
        public ObservableCollection<Chart> WeeklyExpenseChartData { get; set; } = new();
        public ObservableCollection<Chart> WeeklyIncomeChartData { get; set; } = new();

        public AdvicePageViewModel(ParichkoDbContext context)
        {
            _context = context;
            CustomBrushes = new ObservableCollection<Brush>
            {
                new SolidColorBrush(Color.FromArgb("#F7E241")),
                new SolidColorBrush(Color.FromArgb("#FF7A7A")),
                new SolidColorBrush(Color.FromArgb("#CD17FF")),
                new SolidColorBrush(Color.FromArgb("#C2F0FC")),
                new SolidColorBrush(Color.FromArgb("#77BBA2"))
            };
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
            expenses = expenses.OrderByDescending(e => e.Date)
                    .Take(5)
                    .ToList();
                return data;
        }
        public async Task LoadExpenseCategoryChart()
        {
            try
            {
                var data = await LoadExpByCatAsync();

                CategoryExpenseData.Clear();

                foreach (var item in data)
                    CategoryExpenseData.Add(item);
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                });
            }
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
        public async void LoadWeeklyIncomes()
        {
            WeeklyIncomeChartData.Clear();

            var today = DateTimeOffset.Now.Date;
            var last7Days = Enumerable.Range(0, 7)
                                      .Select(i => today.AddDays(-i))
                                      .OrderBy(d => d)
                                      .ToList();

            int userId = Preferences.Get("LoggedUserId", 0);
            var allIncomes = await _context.Incomes
                .Where(e => e.UserProfileId == userId)
                .ToListAsync();
            string day = String.Empty;
            string dayBG = String.Empty;
            foreach (var date in last7Days)
            {
                //decimal total = 20;
                decimal total = allIncomes
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
                WeeklyIncomeChartData.Add(new Chart
                {
                    Day = dayBG,
                    Amount = total
                });
            }
        }
    }
}

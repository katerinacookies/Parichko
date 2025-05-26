using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public AdvicePageViewModel(ParichkoDbContext context)
        {
            _context = context;
            CustomBrushes = new ObservableCollection<Brush>
            {
                new SolidColorBrush(Color.FromArgb("#F7E241")),
                new SolidColorBrush(Color.FromArgb("#FF7A7A")),
                new SolidColorBrush(Color.FromArgb("#F0BCFF")),
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
                data.OrderByDescending(ec => ec.TotalExpense)
                    .Take(5);
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
    }
}

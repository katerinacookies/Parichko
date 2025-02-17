using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class ExpenseContext : ICrudDb<Expense, int>
    {
        public readonly ParichkoDbContext _context;
        public ExpenseContext(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Expense item)
        {
            Expense expense = _context.Expenses.Find(item.Id);
            if (expense == null)
            {
                _context.Expenses.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This expense already exists");
            }
        }

        public async Task DeleteAsync(int key)
        {
            Expense expense = _context.Expenses.Find(key);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This expense doesn't exist");
            }
        }

        public async Task<ICollection<Expense>> ReadAllAsync()
        {
            try
            {
                IQueryable<Expense> query = _context.Expenses;

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
        }

        public async Task<Expense> ReadAsync(int key)
        {
            try
            {
                IQueryable<Expense> expense = _context.Expenses;
                return await expense.Where(a => a.Id == key).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }

        }

        //expense nqma update
        public async Task UpdateAsync(Expense item)
        {
            
        }
    }
}

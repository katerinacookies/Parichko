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
    public class IncomeContext : ICrudDb<Income, int>
    {
        public readonly ParichkoDbContext _context;
        public IncomeContext(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Income item)
        {
            Income income = _context.Incomes.Find(item.Id);
            if (income == null)
            {
                _context.Incomes.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This income already exists");
            }
        }

        public async Task DeleteAsync(int key)
        {
            Income income = _context.Incomes.Find(key);
            if (income != null)
            {
                _context.Incomes.Remove(income);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This income doesn't exist");
            }
        }

        public async Task<ICollection<Income>> ReadAllAsync()
        {
            try
            {
                IQueryable<Income> query = _context.Incomes;

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
        }

        public async Task<Income> ReadAsync(int key)
        {
            try
            {
                IQueryable<Income> incomes = _context.Incomes;
                return await incomes.Where(a => a.Id == key).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }

        }

        public async Task UpdateAsync(Income item)
        {
            //nqma update
        }
    }
}

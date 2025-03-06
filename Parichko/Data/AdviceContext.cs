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
    public class AdviceContext : ICrudDb<Advice, int>
    {
        public readonly ParichkoDbContext _context;
        public AdviceContext(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Advice item)
        {
            Advice advice = _context.Advices.Find(item.Id);
            if (advice == null)
            {
                _context.Advices.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This advice already exists");
            }
        }

        public async Task DeleteAsync(int key)
        {
            Advice advice = _context.Advices.Find(key);
            if (advice != null)
            {
                _context.Advices.Remove(advice);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This advice doesn't exist");
            }
        }

        public async Task<ICollection<Advice>> ReadAllAsync()
        {
            try
            {
                IQueryable<Advice> query = _context.Advices;

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
        }

        public async Task<Advice> ReadAsync(int key)
        {
            try
            {
                IQueryable<Advice> advices = _context.Advices;
                return await advices.Where(a => a.Id == key).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }

        }

        public async Task UpdateAsync(Advice item)
        {
            Advice oldAdvice = await ReadAsync(item.Id);
            oldAdvice.Content = item.Content;
            oldAdvice.Type = item.Type;
            oldAdvice.IsRead = item.IsRead;

            _context.Advices.Update(oldAdvice);
            await _context.SaveChangesAsync();
        }
    }
}

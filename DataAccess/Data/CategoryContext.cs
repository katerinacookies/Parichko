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
    public class CategoryContext : ICrudDb<Category, int>
    {
        public readonly ParichkoDbContext _context;
        public CategoryContext(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category item)
        {
            Category category = _context.Categories.Find(item.Id);
            if (category == null)
            {
                _context.Categories.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This category already exists");
            }
        }

        public async Task DeleteAsync(int key)
        {
            Category category = _context.Categories.Find(key);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This advice doesn't exist");
            }
        }

        public async Task<ICollection<Category>> ReadAllAsync()
        {
            try
            {
                IQueryable<Category> query = _context.Categories;

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
        }

        public async Task<Category> ReadAsync(int key)
        {
            try
            {
                IQueryable<Category> categories = _context.Categories;
                return await categories.Where(c => c.Id == key).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }

        }

        //kategoriite nqmat update
        public async Task UpdateAsync(Category item)
        {
            
        }
    }
}

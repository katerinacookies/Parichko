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
    public class LoginContext : ICrudDb<Login, int>
    {
        public readonly ParichkoDbContext _context;
        public LoginContext(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Login item)
        {
            Login login = _context.Logins.Find(item.Id);
            if (login == null)
            {
                _context.Logins.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This login already exists");
            }
        }
        public async Task DeleteAsync(int key)
        {
            Login login = _context.Logins.Find(key);
            if (login != null)
            {
                _context.Logins.Remove(login);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This login doesn't exist");
            }
        }

        public async Task<ICollection<Login>> ReadAllAsync()
        {
            try
            {
                IQueryable<Login> query = _context.Logins;

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
        }

        public async Task<Login> ReadAsync(int key)
        {
            try
            {
                IQueryable<Login> logins = _context.Logins;
                return await logins.Where(a => a.Id == key).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }
        }

        public async Task UpdateAsync(Login item)
        {
            Login oldLogin = await ReadAsync(item.Id);
            oldLogin.Email = item.Email;
            oldLogin.PasswordHash = item.PasswordHash;
            oldLogin.IsActive = item.IsActive;

            _context.Logins.Update(oldLogin);
            await _context.SaveChangesAsync();
        }

    }
}

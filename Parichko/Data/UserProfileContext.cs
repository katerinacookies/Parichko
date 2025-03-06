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
    public class UserProfileContext : ICrudDb<UserProfile, int>
    {
        public readonly ParichkoDbContext _context;
        public UserProfileContext(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UserProfile item)
        {
            UserProfile userProfile = _context.UserProfiles.Find(item.Id);
            if (userProfile == null)
            {
                _context.UserProfiles.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This user profile already exists");
            }
        }

        public async Task DeleteAsync(int key)
        {
            UserProfile userProfile = _context.UserProfiles.Find(key);
            if (userProfile != null)
            {
                //?
                Login login = _context.Logins.Where(l => l.UserProfile.Id == userProfile.Id).FirstOrDefault();
                _context.Logins.Remove(login);
                _context.UserProfiles.Remove(userProfile);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This user profile doesn't exist");
            }
        }

        public async Task<ICollection<UserProfile>> ReadAllAsync()
        {
            try
            {
                IQueryable<UserProfile> query = _context.UserProfiles;

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
        }

        public async Task<UserProfile> ReadAsync(int key)
        {
            try
            {
                IQueryable<UserProfile> userProfiles = _context.UserProfiles;
                return await userProfiles.Where(a => a.Id == key).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }

        }

        public async Task UpdateAsync(UserProfile item)
        {
            UserProfile oldProfile = await ReadAsync(item.Id);
            oldProfile.DisplayName = item.DisplayName;
            oldProfile.ProfilePic = item.ProfilePic;
            oldProfile.Expenses = item.Expenses;
            oldProfile.Incomes = item.Incomes;
            oldProfile.Advices = item.Advices;
            oldProfile.Friends = item.Friends;
            oldProfile.FontSize = item.FontSize;
            oldProfile.HighContrast = item.HighContrast;
            oldProfile.Theme = item.Theme;
            oldProfile.BannersOn = item.BannersOn;
            oldProfile.SoundOn = item.SoundOn;
            oldProfile.FriendRequests = item.FriendRequests;

            _context.UserProfiles.Update(oldProfile);
            await _context.SaveChangesAsync();
        }
    }
}

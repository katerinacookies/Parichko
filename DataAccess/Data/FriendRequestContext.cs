using DataAccess.Models;
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
    public class FriendRequestContext
    {
        public readonly ParichkoDbContext _context;
        public FriendRequestContext(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(FriendRequest item)
        {
            FriendRequest friendRequest = _context.FriendRequests.Find(item.Id);
            if (friendRequest == null)
            {
                _context.FriendRequests.Add(item);
                UserProfile toUser = _context.UserProfiles.Where(u => u.Id == item.ToUserId).FirstOrDefault();
                toUser.FriendRequests.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This friend request already exists");
            }
        }

        public async Task DeleteAsync(int key)
        {
            FriendRequest friendRequest = _context.FriendRequests.Find(key);
            if (friendRequest != null)
            {
                _context.FriendRequests.Remove(friendRequest);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This advice doesn't exist");
            }
        }

        public async Task<ICollection<FriendRequest>> ReadAllAsync()
        {
            try
            {
                IQueryable<FriendRequest> query = _context.FriendRequests;

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
        }

        public async Task<FriendRequest> ReadAsync(int key)
        {
            try
            {
                IQueryable<FriendRequest> friendRequests = _context.FriendRequests;
                return await friendRequests.Where(a => a.Id == key).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }

        }

        public async Task UpdateAsync(FriendRequest item)
        {
            FriendRequest oldRequest = await ReadAsync(item.Id);
            oldRequest.Status = item.Status;

            _context.FriendRequests.Update(oldRequest);
            await _context.SaveChangesAsync();
        }
    }
}

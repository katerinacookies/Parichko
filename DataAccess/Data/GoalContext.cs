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
    public class GoalContext
    {
        public readonly ParichkoDbContext _context;
        public GoalContext(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Goal item)
        {
            Goal goal = _context.Goals.Find(item.Id);
            if (goal == null)
            {
                _context.Goals.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This goal already exists");
            }
        }

        public async Task DeleteAsync(int key)
        {
            Goal goal = _context.Goals.Find(key);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("This goal doesn't exist");
            }
        }

        public async Task<ICollection<Goal>> ReadAllAsync()
        {
            try
            {
                IQueryable<Goal> query = _context.Goals;

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
        }

        public async Task<Goal> ReadAsync(int key)
        {
            try
            {
                IQueryable<Goal> goals = _context.Goals;
                return await goals.Where(a => a.Id == key).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong");
            }

        }

        public async Task UpdateAsync(Goal item)
        {
            Goal oldGoal = await ReadAsync(item.Id);
            oldGoal.Name = item.Name;
            oldGoal.GoalAmount = item.GoalAmount;
            oldGoal.SavedAmount = item.SavedAmount;
            oldGoal.SavedPercent = item.SavedPercent;
            oldGoal.IconName = item.IconName;
            oldGoal.Starred = item.Starred;
            oldGoal.Savers = item.Savers;

            _context.Goals.Update(oldGoal);
            await _context.SaveChangesAsync();
        }

    }
}

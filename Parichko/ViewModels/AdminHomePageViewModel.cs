using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class AdminHomePageViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<UserProfile> Users { get; set; } = new();

        public AdminHomePageViewModel(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoadUsersAsync()
        {
            try
            {
                var usersFromDb = _context.UserProfiles
                                     .Include(up => up.Login).ToList();

                Users.Clear();

                foreach (var user in usersFromDb)
                {
                    Users.Add(user);
                }
                return true;
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                });
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<bool> DeleteProfile(int upId)
        {
            var upForDelete = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.Id == upId);

            if (upForDelete == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Не е намерен такъв потребител.", "Добре");
                });
                return false;
            }

            try
            {
                //изтриване на всички разходи
                var expenses = await _context.Expenses
                    .Where(e => e.UserProfileId == upId)
                    .ToListAsync();
                foreach (var expense in expenses)
                {
                    _context.Expenses.Remove(expense);
                }

                //изтриване на покани за приятелство
                var requests = await _context.FriendRequests
                    .Include(r => r.FromUser)
                    .Where(e => e.ToUserId == upId || e.FromUser.Id == upId)
                    .ToListAsync();
                foreach (var request in requests)
                {
                    _context.FriendRequests.Remove(request);
                }

                //изтриване на цели
                var usergoalsOfUser = await _context.UserGoals
                    .Where(ug => ug.UserProfileId == upId)
                    .ToListAsync();
                foreach (var usergoal in usergoalsOfUser)
                {
                    var goal = await _context.Goals
                        .Where(g => g.Id == usergoal.GoalId)
                        .FirstOrDefaultAsync();
                    _context.Goals.Remove(goal);
                    _context.UserGoals.Remove(usergoal);
                }

                //изтриване на приходи
                var incomes = await _context.Incomes
                    .Where(i => i.UserProfileId == upId)
                    .ToListAsync();
                foreach (var income in incomes)
                {
                    _context.Incomes.Remove(income);
                }

                //изтриване на login
                var login = await _context.Logins
                    .Include(l => l.UserProfile)
                    .Where(l => l.UserProfile.Id == upId)
                    .FirstOrDefaultAsync();
                
                Users.Remove(upForDelete);
                _context.UserProfiles.Remove(upForDelete);
                _context.Logins.Remove(login);

                /*var goalsWithUser = await _context.Goals
                    .Include(g => g.Savers)
                    .Where(g => g.Savers.Contains(upForDelete))
                    .ToListAsync();*/
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                });
                return false;
            }
        }
    }
}

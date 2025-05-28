//using Android.Webkit;
using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class GoalViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<Goal> Goals { get; set; } = new();
        public ObservableCollection<UserProfile> Friends { get; set; } = new();
        public GoalViewModel(ParichkoDbContext context)
        {
            _context = context;
        }
        public async Task<bool> LoadGoalsAsync()
        {
            /*try
            {
                List<UserGoal> userGoals = new List<UserGoal>();
                userGoals = await _context.UserGoals
                    .Where(ug => ug.UserProfileId == Preferences.Get("LoggedUserId", 0))
                    .ToListAsync();

                List<Goal> goalsFromDb = new List<Goal>();

                foreach (UserGoal usergoal in userGoals)
                {
                    var goal = await _context.Goals.FirstOrDefaultAsync(g => g.Id == usergoal.GoalId);
                    goalsFromDb.Add(goal);
                }

                Goals.Clear();

                foreach (var goal in goalsFromDb)
                {
                    Goals.Add(goal);
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
            }*/
            try
            {
                int userId = Preferences.Get("LoggedUserId", 0);
                Debug.WriteLine($"[LoadGoalsAsync] LoggedUserId = {userId}");
                
                var currentUser = await _context.UserProfiles
                    .Include(up => up.Friends)
                    .FirstOrDefaultAsync(up => up.Id == userId);

                if(currentUser == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                        Shell.Current.DisplayAlert("Грешка", "Текущият потребител не е намерен.", "Добре"));
                }

                foreach(UserProfile friend in currentUser.Friends)
                {
                    Friends.Add(friend);
                }

                var userGoals = await _context.UserGoals
                    .Where(ug => ug.UserProfileId == userId)
                    .Include(ug => ug.Goal)
                    .ToListAsync();

                Debug.WriteLine($"[LoadGoalsAsync] userGoals.Count = {userGoals.Count}");

                Goals.Clear();
                foreach (var ug in userGoals)
                {
                    if (ug.Goal != null)
                    {
                        Goals.Add(ug.Goal);
                        Debug.WriteLine($"[LoadGoalsAsync] Цел: {ug.Goal.Name}, ID: {ug.Goal.Id}");
                    }
                    else
                    {
                        Debug.WriteLine($"[LoadGoalsAsync] null Goal при UserGoal с GoalId: {ug.GoalId}");
                    }
                }

                Debug.WriteLine($"[LoadGoalsAsync] Goals.Count = {Goals.Count}");
                return true;
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре"));
                return false;
            }
        }

        
        public async Task<bool> AddGoalsAsync(string name, decimal amount, string icon, string color)
        {
            try
            {
                if (_context == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Контекстът е null.", "Добре");
                    });
                    return false;
                }
                name = name.Trim();
                icon = icon.Trim();
                color = color.Trim();


                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(icon) || string.IsNullOrWhiteSpace(color))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                        Shell.Current.DisplayAlert("Грешка", "Попълнете всички полета!", "Добре"));
                    return false;
                }


                await Task.Run(async () =>
                {
                    Goal newGoal = new Goal();
                    newGoal.Name = name;
                    newGoal.GoalAmount = amount;
                    newGoal.SavedAmount = 0;
                    newGoal.SavedPercent = 0;
                    newGoal.IconName = icon;
                    newGoal.Starred = false;
                    newGoal.Color = color;

                    try
                    {
                        _context.Goals.Add(newGoal);
                        await _context.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре"));
                        return false;
                    }

                    int userId = Preferences.Get("LoggedUserId", 0);
                    var currentUser = await _context.UserProfiles.FindAsync(userId);
                    //var currentUser = await _context.UserProfiles.FirstOrDefault(up => up.Id == userId);
                    if (currentUser == null)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                            Shell.Current.DisplayAlert("Грешка", "Не е намерен текущият потребител!", "Добре"));
                        return false;
                    }

                    UserGoal newUG = new UserGoal();
                    newUG.GoalId = newGoal.Id;
                    newUG.UserProfileId = userId;
                    newUG.Goal = newGoal;
                    newUG.UserProfile = currentUser;

                    try
                    {
                        _context.UserGoals.Add(newUG);
                        await _context.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре"));
                        return false;
                    }

                    //обновява UI
                    Goals.Add(newGoal);
                    return true;
                });
                return true;
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                });

                if (ex.InnerException != null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", ex.InnerException.Message, "Добре");
                    });
                    if (ex.InnerException.InnerException != null)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Shell.Current.DisplayAlert("Грешка", ex.InnerException.InnerException.Message, "Добре");
                        });
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            await MainThread.InvokeOnMainThreadAsync(() =>
                            {
                                Shell.Current.DisplayAlert("Грешка", ex.InnerException.InnerException.InnerException.Message, "Добре");
                            });
                        }
                    }
                }
                return false;
            }
        }

        public async Task<bool> DeleteGoal(int goalId)
        {
            var currentGoal = await _context.Goals
                    .FirstOrDefaultAsync(c => c.Id == goalId);
            var currentUG = await _context.UserGoals
                .Where(ug => ug.GoalId == goalId)
                .ToListAsync();

            if (currentGoal == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Няма такава цел.", "Добре");
                });
                return false;
            }

            foreach(UserGoal usergoal in currentUG)
            {
                int userId = usergoal.UserProfileId;
                var user = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.Id == userId);
                user.Goals.Remove(usergoal);

                _context.UserGoals.Remove(usergoal);
            }
            try
            {
                Goals.Remove(currentGoal);

                    _context.Goals.Remove(currentGoal);
                

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
        public async Task<bool> UpdateSavedAmountAsync(Goal goal, decimal addedAmount)
        {
            try
            {
                goal.SavedAmount += addedAmount;

                await _context.SaveChangesAsync();
                await MainThread.InvokeOnMainThreadAsync(() =>
                        Shell.Current.DisplayAlert("Готово", "Напредъкът за тази цел е добавен.", "Добре"));
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

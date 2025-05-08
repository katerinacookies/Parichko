using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class GoalViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<Goal> Goals { get; set; } = new();

        public GoalViewModel(ParichkoDbContext context)
        {
            _context = context;
        }
        public async Task<bool> LoadGoalsAsync()
        {
            try
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
                        Shell.Current.DisplayAlert("Грешка", "Контекстът е нъл.", "Добре");
                    });
                    return false;
                }
                name = name.Trim();
                icon = icon.Trim();
                color = color.Trim();
                

                if (string.IsNullOrWhiteSpace(icon))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Попълнете всички полета!", "Добре");
                    });
                    return false;
                }
                if (string.IsNullOrWhiteSpace(color))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Попълнете всички полета!", "Добре");
                    });
                    return false;
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Попълнете всички полета!", "Добре");
                    });
                    return false;
                }


                await Task.Run(async () =>
                {
                    Goal newGoal = new Goal();
                    newGoal.Name = name;
                    newGoal.GoalAmount = amount;


                    var currentUser = await Task.Run(async () =>
                        _context.UserProfiles.FirstOrDefault(up => up.Id == Preferences.Get("LoggedUserId", 0)));
                    newGoal.Savers.Add(currentUser);

                    _context.Goals.Add(newGoal);

                    UserGoal ug = new UserGoal();
                    ug.UserProfileId = Preferences.Get("LoggedUserId", 0);
                    ug.GoalId = newGoal.Id;

                    _context.UserGoals.Add(ug);

                    try
                    {
                        _context.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Целта е добавена!");
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
    }
}

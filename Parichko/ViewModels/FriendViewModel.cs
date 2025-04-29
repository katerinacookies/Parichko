using DataAccess.Models;
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
    public class FriendViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<FriendRequest> FriendRequests { get; set; } = new();
        public ObservableCollection<UserProfile> Friends { get; set; } = new();

        public FriendViewModel(ParichkoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoadRequestsAsync()
        {
            try
            {
                var requestsFromDb = _context.FriendRequests
                    .Include(r => r.FromUser)
                    .Where(r => r.ToUserId == Preferences.Get("LoggedUserId", 0) && r.Status == DataAccess.Recources.Status.Pending)
                    .ToList();

                FriendRequests.Clear();

                foreach (var request in requestsFromDb)
                {
                    FriendRequests.Add(request);
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

        //Добавяне на приятелите в списък
        public async Task<bool> LoadFriendsAsync()
        {
            try
            {
                var currentUser = await _context.UserProfiles
                    .Include(u => u.Friends)
                    .FirstOrDefaultAsync(u => u.Id == Preferences.Get("LoggedUserId", 0));
                

                Friends.Clear();

                foreach (var friend in currentUser.Friends)
                {
                    Friends.Add(friend);
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

        //Добавяне на нов приятел
        public async Task<bool> AddFriend(string email)
        {
            try
            {
                var userFromDb = await _context.UserProfiles
                    .Include(u => u.Login)
                    .FirstOrDefaultAsync(u => u.Login.Email == email);

                if (userFromDb == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Не съществува потребител с такъв имейл.", "Добре");
                    });
                    return false;
                }

                int currentUserId = Preferences.Get("LoggedUserId", 0);

                var currentUser = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == currentUserId);

                if (email == currentUser.Login.Email)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Не може да пратите покана на себе си.", "Добре");
                    });
                    return false;
                }

                var requestFromDb = await _context.FriendRequests.FirstOrDefaultAsync(u => u.ToUserId == userFromDb.Id && u.FromUser.Id == currentUserId);


                if (requestFromDb != null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Вече сте изпратили покана на този потребител.", "Добре");
                    });
                    return false;
                }

               // int reqfdbid = requestFromDb.Id;

                try
                {
                    FriendRequest newFriendRequest = new FriendRequest();

                    newFriendRequest.FromUser = currentUser;
                    newFriendRequest.ToUser = userFromDb;
                    newFriendRequest.ToUserId = userFromDb.Id;
                    newFriendRequest.Status = DataAccess.Recources.Status.Pending;
                    

                    await _context.FriendRequests.AddAsync(newFriendRequest);
                    

                    if (userFromDb.FriendRequests == null)
                    {
                        userFromDb.FriendRequests = new List<FriendRequest>();
                    }
                    userFromDb.FriendRequests.Add(newFriendRequest);

                    await _context.SaveChangesAsync();

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Успех", "Май стана", "Добре");
                    });

                    return true;
                }
                catch (Exception ex)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                    });
                    if(ex.InnerException != null)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Shell.Current.DisplayAlert("Грешка", ex.InnerException.Message, "Добре");
                        });
                    }
                    return false;
                }
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
                }
                return false;
            }
        }
    }
}

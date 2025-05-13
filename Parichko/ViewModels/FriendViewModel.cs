using DataAccess.Models;
//using Kotlin.Properties;
using Microsoft.EntityFrameworkCore;
using Parichko.Data;
using Parichko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.ViewModels
{
    public class FriendViewModel
    {
        private readonly ParichkoDbContext _context;
        public ObservableCollection<FriendRequest> FriendRequests { get; set; } = new();
        public ObservableCollection<UserProfile> Friends { get; set; } = new();
        private int friendCount;
        public int FriendCount
        {
            get => friendCount;
            set
            {
                if (friendCount != value)
                {
                    friendCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public FriendViewModel(ParichkoDbContext context)
        {
            _context = context;
            Friends.CollectionChanged += (s, e) =>
            {
                FriendCount = Friends.Count;
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public async Task<bool> LoadRequestsAsync()
        {
            try
            {
                var requestsFromDb = _context.FriendRequests
                    .Include(r => r.FromUser.Login)
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

                var currentUser = await _context.UserProfiles
                    .Include (u => u.Login)
                    .FirstOrDefaultAsync(u => u.Id == currentUserId);

                if(currentUser == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Не е намерен текущият потребител.", "Добре");
                    });
                    return false;
                }

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
                        Shell.Current.DisplayAlert("Успех", "Поканата е изпратена!", "Добре");
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

        //Отхвърляне на покана
        public async Task<bool> DenyRequest(int fromUserId)
        {
            int currentUserId = Preferences.Get("LoggedUserId", 0);

            var currentRequest = await _context.FriendRequests
                    .FirstOrDefaultAsync(fr => fr.ToUser.Id == currentUserId && fr.FromUser.Id == fromUserId);

            if(currentRequest == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Няма такава покана.", "Добре");
                });
                return false;
            }

            try
            {
                FriendRequests.Remove(currentRequest);
                var currentUser = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.Id == currentUserId);
                if(currentUser == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Няма такъв потребител.", "Добре");
                    });
                    return false;
                }

                currentUser.FriendRequests.Remove(currentRequest);

                _context.FriendRequests.Remove(currentRequest);
                await _context.SaveChangesAsync();

                
                return true;
            }
            catch(Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", ex.Message, "Добре");
                });
                return false;
            }
        }

        //Приемане на покана
        public async Task<bool> AcceptRequest(int fromUserId)
        {
            int currentUserId = Preferences.Get("LoggedUserId", 0);

            var currentRequest = await _context.FriendRequests
                    .FirstOrDefaultAsync(fr => fr.ToUser.Id == currentUserId && fr.FromUser.Id == fromUserId);

            if (currentRequest == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Няма такава покана.", "Добре");
                });
                return false;
            }

            try
            {
                var currentUser = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.Id == currentUserId);
                if (currentUser == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Няма такъв потребител.", "Добре");
                    });
                    return false;
                }

                var fromUser = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.Id == fromUserId);

                if(fromUser == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Няма такъв потребител..", "Добре");
                    });
                    return false;
                }

                currentUser.Friends.Add(fromUser);
                fromUser.Friends.Add(currentUser);

                FriendRequests.Remove(currentRequest);
                Friends.Add(fromUser);
                currentUser.FriendRequests.Remove(currentRequest);

                _context.FriendRequests.Remove(currentRequest);
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

        //
        public async Task<bool> RemoveFriend(int friendId)
        {
            int currentUserId = Preferences.Get("LoggedUserId", 0);

            var friend = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.Id == friendId);

            if (friend == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.DisplayAlert("Грешка", "Няма такъв потребител.", "Добре");
                });
                return false;
            }

            try
            {
                Friends.Remove(friend);
                var currentUser = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.Id == currentUserId);
                if (currentUser == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.DisplayAlert("Грешка", "Няма такъв потребител.", "Добре");
                    });
                    return false;
                }

                currentUser.Friends.Remove(friend);
                friend.Friends.Remove(currentUser);

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

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
    }
}

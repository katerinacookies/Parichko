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

        public async void LoadUsers()
        {
            var usersFromDb = _context.UserProfiles.ToList();

            Users.Clear();

            foreach (var user in usersFromDb)
            {
                Users.Add(user);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

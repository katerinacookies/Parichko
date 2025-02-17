using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public Login Login { get; set; }
        public string ProfilePic { get; set; } = "default.jpg";
        public IList<Expense> Expenses { get; set; } = new List<Expense>();
        public IList<Income> Incomes { get; set; } = new List<Income>();
        //ilist ili icollection?
        public IList<Advice> Advices { get; set; } = new List<Advice>();
        //?
        public IList<UserProfile> Friends { get; set; } = new List<UserProfile>();
        public IList<UserGoal> Goals { get; }

        //realm user id za associaciq
        //public string UserId { get; set; } = string.Empty;

        //настройки за достъпност
        public int FontSize { get; set; } = 14; //po podrazbirane
        public bool HighContrast { get; set; } = false;
        public bool Theme { get; set; } = false; //0 - svetla tema

        //izvestiq
        public bool BannersOn { get; set; } = true;
        public bool SoundOn { get; set; } = true;

        //покани за приятелство
        public IList<FriendRequest> FriendRequests { get; set; } = new List<FriendRequest>();
    }
}

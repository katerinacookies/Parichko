using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Parichko.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal GoalAmount { get; set; }
        public decimal SavedAmount { get; set; }
        public short SavedPercent { get; set; }
        public string IconName { get; set; } = "default.png";
        public string Color { get; set; }
        public bool Starred { get; set; } = false;

        //public IList<UserProfile> Savers { get; set; } = new List<UserProfile>();
        public IList<UserGoal> UGoals { get; set; } = new List<UserGoal>();
        public ICollection<UserGoal> Savers { get; set; } = new List<UserGoal>();
    }
}

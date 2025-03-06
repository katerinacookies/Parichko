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

        //dali da e spisuk s potrebitelq ili prazen spisuk (samo za priqtelite mu)
        public IList<UserGoal> Savers { get; set; } = new List<UserGoal>();
    }
}

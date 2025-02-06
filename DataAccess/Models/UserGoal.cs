using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.Models
{
    public class UserGoal
    {
        public int UserProfileId { get; set; }
        public int GoalId { get; set; }
        public UserProfile UserProfile { get; set; }
        public Goal Goal { get; set; }
    }
}

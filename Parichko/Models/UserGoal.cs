using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.Models
{
    [Table("UserGoals")] 
    // уверява EF, че се използва правилното име на таблицата
    public class UserGoal
    {
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        [ForeignKey("Goal")]
        public int GoalId { get; set; }

        public UserProfile UserProfile { get; set; }
        public Goal Goal { get; set; }
    }
}

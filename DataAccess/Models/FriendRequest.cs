using DataAccess.Recources;
using Parichko.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class FriendRequest
    {
        [Key]
        public int Id { get; set; }
        public UserProfile FromUser { get; set; }
        public UserProfile ToUser { get; set; }
        public int ToUserId { get; set; }
        public Status Status { get; set; }
    }
}

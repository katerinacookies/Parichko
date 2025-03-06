using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.Models
{
    public class Advice
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(400)]
        public string Content { get; set; } = string.Empty;

        [MaxLength(50)]
        //required?
        public string Type { get; set; }

        public DateTimeOffset DateGenerated { get; set; } = DateTimeOffset.Now;

        public bool IsRead { get; set; } = false;

        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
    }
}

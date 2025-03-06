using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
    }
}

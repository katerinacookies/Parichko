using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Parichko.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Amount { get; set; }
        //trqbva da se napravi stoinost po podrazbirane
        public int CategoryId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }

    }
}

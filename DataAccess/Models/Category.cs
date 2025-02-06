using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //cvqt po podrazbirane - siv
        public string Color { get; set; } = "#5f5d5d";
        public string IconName { get; set; } = "others.png";
        public IList<Expense> Expenses { get; set; }
    }
}

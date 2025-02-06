using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parichko.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public UserProfile UserProfile { get; set; }
    }
}

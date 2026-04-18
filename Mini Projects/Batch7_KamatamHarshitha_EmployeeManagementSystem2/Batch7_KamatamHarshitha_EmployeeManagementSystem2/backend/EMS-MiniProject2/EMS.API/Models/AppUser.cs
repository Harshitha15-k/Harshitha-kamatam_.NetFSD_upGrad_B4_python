using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.API.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; } // We will store BCrypt hashes here, NEVER plain text

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } // "Admin" or "Viewer"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
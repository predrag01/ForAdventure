using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForAdventure.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string NameInApplication { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string?  ImageURL { get; set; }
        [Required]
        public DateTime DateRegistration { get; set; }=DateTime.Now;
        [NotMapped]
        public string? Role { get; set; }
        [NotMapped]
        public int ReportsCount { get; set; }
    }
}

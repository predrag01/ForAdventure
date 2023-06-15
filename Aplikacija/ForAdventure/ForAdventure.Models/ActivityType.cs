using System.ComponentModel.DataAnnotations;

namespace ForAdventure.Models
{
    public class ActivityType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Type { get; set; }
    }
}

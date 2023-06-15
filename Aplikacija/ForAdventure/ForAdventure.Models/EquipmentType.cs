using System.ComponentModel.DataAnnotations;

namespace ForAdventure.Models
{
    public class EquipmentType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Naziv { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ForAdventure.Models
{
    public class Equipment
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Description")]
        public string? Opis { get; set; }
        [Required]
        [DisplayName("Location")]
        public string? Lokacija { get; set; }
        [Required]
        [DisplayName ("Address")]
        public string? Adresa { get; set; }
        [Required]
        [DisplayName("Name")]
        public string? Naziv { get; set; }
        [Required]
        [DisplayName("Price")]
        public int Cena { get; set; }
        [NotMapped]
        [DisplayName("Images")]
        public List<string>? ImageURLs { get; set; }
        public string ImageURLsJson
        {
            get => JsonSerializer.Serialize(ImageURLs);
            set => ImageURLs = JsonSerializer.Deserialize<List<string>>(value);
        }
        public List<EquipmentReservation>? Reservations { get; set; }
        public int EqTypeId { get; set; }
        public EquipmentType? EqType { get; set; }
        public string? OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }

        
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ForAdventure.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Address { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number must be in format XXX XXX XXXX")]
        public string Phone { get; set; }
        public string? Email { get; set; }
        public bool Pool { get; set; }
        public bool Gym { get; set; }
        public bool Spa { get; set; }
        public bool Sauna { get; set; }
        public bool Restaurant { get; set; }
        [NotMapped]
        [DisplayName("Images")]
        public List<string>? ImageURLs { get; set; }
        public string? ImageURLsJson
        {
            get => JsonSerializer.Serialize(ImageURLs);
            set => ImageURLs = JsonSerializer.Deserialize<List<string>>(value);
        }
        public bool Parking { get; set; }
        public List<Room>? Rooms { get; set; }
        [Required]
        public string? OwnerId{ get; set; }
        public ApplicationUser? Owner { get; set; }

    }
}

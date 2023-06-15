using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForAdventure.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int NumOfBeds { get; set; }
        public float PricePerNight { get; set; }
        public List<RoomReservation>? Reservations { get; set; }
        [Required]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        [Required]
        [DisplayName("Room type")]
        public int RoomTypeId { get; set; }
        [DisplayName("Room type")]
        public RoomType RoomType { get; set; }
    }
}

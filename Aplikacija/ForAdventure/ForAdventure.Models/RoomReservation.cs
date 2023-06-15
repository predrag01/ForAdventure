using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForAdventure.Models
{
    public class RoomReservation
    {
        [Key]
        public int Id { get; set; }
        public DateTime? ReservedFrom { get; set; }
        public DateTime? ReservedUntil { get; set; }
        public int RoomId { get; set; }
		[ForeignKey("RoomId")]
		[ValidateNever]
		public Room Room { get; set; }
        public string UserId { get; set; }
		[ForeignKey("UserId")]
		[ValidateNever]
		public ApplicationUser? User { get; set; }
        public DateTime? DateReserved { get; set; } = DateTime.Now;
		public double TotalPrice { get; set; }
    }
}

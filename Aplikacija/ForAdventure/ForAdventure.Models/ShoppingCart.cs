using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForAdventure.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        public int? ActivityId { get; set; }
        [ForeignKey("ActivityId")]
        [ValidateNever]
        public Activity? Activity { get; set; }
        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int? NumberOfPersons { get; set; }
        [NotMapped]
        public double PriceActivity { get; set; }

        public int? EquipmentId { get; set; }
        [ForeignKey("EquipmentId")]
        [ValidateNever]
        public Equipment? Equipment { get; set; }
        public DateTime? DateFromEquipment { get; set; }
        public DateTime? DateToEquipment { get; set; }
        [NotMapped]
        public double PriceEquipment { get; set; }
        [NotMapped]
        public Hotel? Hotel { get; set; }

        public int? RoomId { get; set; }
        [ForeignKey("RoomId")]
        [ValidateNever]
        public Room? Room { get; set; }
        public DateTime? DateFromRoom { get; set; }
        public DateTime? DateToRoom { get; set; }
		[NotMapped]
		public double PriceHotel { get; set; }

		public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public int DaysHotel { get; set; }
        [NotMapped]
        public int DaysEquipment { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForAdventure.Models
{
    public class EquipmentReservation
    {
        [Key]
        public int Id { get; set; }
        public DateTime? ReservedFrom { get; set; }
        public DateTime? ReservedUntil { get; set; }
        public int EquipmentId { get; set; }
        [ForeignKey("EquipmentId")]
        [ValidateNever]
        public Equipment Equipment { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime? DateReserved { get; set; } = DateTime.Now;
        public double TotalPrice { get; set; }
    }
}

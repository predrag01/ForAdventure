using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForAdventure.Models
{
    public class ActivityReservation
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DateReserved { get; set; } = DateTime.Now;
		public int ActivityId { get; set; }
        [ForeignKey("ActivityId")]
        [ValidateNever]
        public Activity Activity { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public double TotalPrice { get; set; }
        public int NumberOfPeople { get; set; }
    }
}

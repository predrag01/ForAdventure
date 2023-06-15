using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForAdventure.Models
{
    public class Report
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[DisplayName("Reason for reporting:")]
		public string Text { get; set; }
		[Required]
		public string ReporterId { get; set; }
		[ForeignKey("ReporterId")]
		[ValidateNever]
        public ApplicationUser? Reporter { get; set; }
		[Required]
		public string ReportedId { get; set; }
		[ForeignKey("ReportedId")]
		[ValidateNever]
		public ApplicationUser? Reported { get; set; }
		[NotMapped]
		public int Count { get; set; }
        public bool Checked { get; set; }
    }
}

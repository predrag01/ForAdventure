using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ForAdventure.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        [DisplayName("Skill Level")]
        public string SkillLevel { get; set; }
        [Required]
		[DisplayName("Date From")]
		public DateTime DateFrom { get; set; }
        [Required]
        [DisplayName("Date To")]
		public DateTime DateTo { get; set; }
        public int CurrentNumberPeople { get; set; }
        [DisplayName("Image")]
        public string? ImageURL { get; set; }

		[Required]
        public int ActivityTypeId { get; set; }
        public ActivityType? ActivityType { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}

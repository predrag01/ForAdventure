namespace ForAdventure.Models.ViewModels
{
    public class UserVM
	{
		public ApplicationUser User { get; set; }
        public IEnumerable<Report> Reports { get; set; }
		public int Count { get; set; }
    }
}

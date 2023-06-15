namespace ForAdventure.Models.ViewModels
{
    public class ProfileVM
	{
		public ApplicationUser User { get; set; }
		public IEnumerable<Activity> ActivityList { get; set; }
		public IEnumerable<Equipment> EquipmentList { get; set;}
		public IEnumerable<Hotel> HotelList { get; set;}
	}
}

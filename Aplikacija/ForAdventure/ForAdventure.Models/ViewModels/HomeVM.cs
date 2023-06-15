namespace ForAdventure.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Activity>? LatestActivities { get; set; }
        public IEnumerable<Hotel>? LatestHotels { get; set; }
        public IEnumerable<Equipment>? LatestEquipment { get; set; }
    }
}

using ForAdventure.Models;

namespace ForAdventureWeb.Areas.Customer.ViewModels
{
    public class ActivitiesViewModel
    {
        public IEnumerable<Activity> Activities { get; set; }
       
        public IEnumerable<ActivityType> ActivityTypes { get; set; }
        public int SelectedActivityTypeId { get; set; }
        public ActivitiesViewModel()
        {
        }
    }
}

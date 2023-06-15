using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;

namespace ForAdventure.Service
{
    public class ActivityReservationService : IActivityReservationService
	{
		private ApplicationDbContext _db;
		public IActivityReservationRepository ActivityReservation { get; private set; }

		public ActivityReservationService(ApplicationDbContext db, IActivityReservationRepository activityReservation)
		{
			_db = db;
			ActivityReservation = activityReservation;
		}

        public IEnumerable<ActivityReservation> GetUsersbyActivity(int? id)
        {
            IEnumerable<ActivityReservation> activities = ActivityReservation.WhereModified(x => x.ActivityId == id);
            return activities;
        }
		public bool Check(int id, int number, int capacity)
		{
			IEnumerable<ActivityReservation> act = ActivityReservation.GetAll().Where(u => u.ActivityId == id);
			int sum = 0;
			foreach(ActivityReservation activity in act)
			{
				sum += activity.NumberOfPeople;
			}
			if(capacity<sum+number)
			{
				return false;
			}
			else
			{
				return true;
			}	
		}
    }
}

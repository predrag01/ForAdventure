using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository
{
    public class ActivityReservationRepository: Repository<ActivityReservation>, IActivityReservationRepository
	{
		private ApplicationDbContext _db;
		public ActivityReservationRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public void Save()
		{
			_db.SaveChanges();
		}

		public void Update(ActivityReservation ActivityReservation)
		{
			_db.ActivityReservations.Update(ActivityReservation);
		}
	}
}

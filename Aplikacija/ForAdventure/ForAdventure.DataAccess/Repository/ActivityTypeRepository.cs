using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository
{
    public class ActivityTypeRepository : Repository<ActivityType>, IActivityTypeRepository
    {
        private ApplicationDbContext _db;
        public ActivityTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

		public void Save()
		{
			_db.SaveChanges();
		}

		public void Update(ActivityType activityType)
        {
            _db.ActivityTypes.Update(activityType);
        }
    }
}

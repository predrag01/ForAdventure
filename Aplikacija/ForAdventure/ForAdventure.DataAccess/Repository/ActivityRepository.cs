using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        private ApplicationDbContext _db;
        public ActivityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Activity activity)
        {
            _db.Activities.Update(activity);
        }
    }
}

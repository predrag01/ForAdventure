
using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;

namespace ForAdventure.Service
{
    public class ActivityTypeService : IActivityTypeService
    {
        private ApplicationDbContext _db;
        public IActivityTypeRepository ActivityType { get; private set; }

        public ActivityTypeService(ApplicationDbContext db, IActivityTypeRepository activityType)
        {
            _db = db;
            ActivityType = activityType;
        }
        public ActivityType GetActivityTypeById(int? id)
        {
            ActivityType activitytype = ActivityType.GetFirstOrDefault(u => u.Id == id);
            return activitytype;
        }


    }
}
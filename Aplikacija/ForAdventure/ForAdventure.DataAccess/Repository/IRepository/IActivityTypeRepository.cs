using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IActivityTypeRepository:IRepository<ActivityType>
    {
        void Update(ActivityType activityType);
		public void Save();
	}
}

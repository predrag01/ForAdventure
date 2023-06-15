using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IActivityRepository:IRepository<Activity>
    {
        public void Save();
        void Update(Activity activity);
    }
}

using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IActivityService
    {
        public IActivityRepository Activity { get; }
		public IEnumerable<Activity> GetActivitiesByUser(string id);
        public Activity GetActivityById(int? id);
        public IEnumerable<Activity> GetActivityByActivityType(int? id);
        bool DeletePicture(string image);
        void RemoveActivity(int id);
    }
}

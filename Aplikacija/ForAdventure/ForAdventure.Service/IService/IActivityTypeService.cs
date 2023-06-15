using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IActivityTypeService
    {
        public IActivityTypeRepository ActivityType { get; }
        public ActivityType GetActivityTypeById(int? id);


    }
}

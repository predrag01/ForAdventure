using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        void Update(Equipment equipment );
        void Save();
    }
}

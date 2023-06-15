using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IEquipmentTypeRepository : IRepository<EquipmentType>
    {
        void Update(EquipmentType equipmentType);
        public void Save();
    }
}

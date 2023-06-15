using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IEquipmentTypeService
    {
        public IEquipmentTypeRepository EquipmentType { get; }
        public EquipmentType GetEquipmentTypeById(int? id);
    }
}


using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;

namespace ForAdventure.Service
{
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private ApplicationDbContext _db;
        public IEquipmentTypeRepository EquipmentType { get; private set; }

        public EquipmentTypeService(ApplicationDbContext db, IEquipmentTypeRepository equipmentType)
        {
            _db = db;
            EquipmentType = equipmentType;
        }
        public EquipmentType GetEquipmentTypeById(int? id)
        {
            EquipmentType equipmenttype = EquipmentType.GetFirstOrDefault(u => u.Id == id);
            return equipmenttype;
        }

    }
}
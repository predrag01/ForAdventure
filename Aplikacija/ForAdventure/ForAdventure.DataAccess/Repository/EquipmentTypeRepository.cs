using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository
{
    public class EquipmentTypeRepository : Repository<EquipmentType>, IEquipmentTypeRepository
    {
        private ApplicationDbContext _db;
        public EquipmentTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(EquipmentType EquipmentType)
        {
            _db.EquipmentTypes.Update(EquipmentType);
        }
    }
}

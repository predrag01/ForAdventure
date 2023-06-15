using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository
{
    public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        private ApplicationDbContext _db;
        public EquipmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Equipment equipment)
        {
            _db.Update(equipment);
        }
    }
}

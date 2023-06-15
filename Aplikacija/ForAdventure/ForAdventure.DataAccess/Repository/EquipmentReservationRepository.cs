using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository
{
    public class EquipmentReservationRepository : Repository<EquipmentReservation>, IEquipmentReservationRepository
    {
        private ApplicationDbContext _db;
        public EquipmentReservationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }
        public void Update(EquipmentReservation reservation)
        {
            _db.EquipmentReservations.Update(reservation);
        }
    }
}

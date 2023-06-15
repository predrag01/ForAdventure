using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository
{
    public class RoomReservationRepository : Repository<RoomReservation>, IRoomReservationRepository
    {
        private ApplicationDbContext _db;
        public RoomReservationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }
        public void Update(RoomReservation roomReservation)
        {
            _db.RoomReservations.Update(roomReservation);
        }
    }
}

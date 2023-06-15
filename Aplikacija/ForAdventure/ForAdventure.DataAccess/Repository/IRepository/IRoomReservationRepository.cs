using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IRoomReservationRepository :IRepository<RoomReservation>
    {
        public void Save();
        void Update(RoomReservation roomReservation);
    }
}

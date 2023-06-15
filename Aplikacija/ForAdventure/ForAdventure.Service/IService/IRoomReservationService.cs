using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IRoomReservationService
    {
        public IRoomReservationRepository RoomReservation { get; }

        public IEnumerable<RoomReservation> GetAllRoomReservationsByRoomId(int? id);
        public bool Check(int id, DateTime from, DateTime to);
    }
}

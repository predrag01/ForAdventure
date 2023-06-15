using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IRoomService
    {
        public IRoomRepository Room { get; }

        public IEnumerable<Room> GetAllRoomsByHotelId(int? id);
        public int RoomCount(int? id);
        public float GetLowestPrice(int? id);
        Room GetRoomById(int? id);
        void RemoveRoom(int id);
    }
}

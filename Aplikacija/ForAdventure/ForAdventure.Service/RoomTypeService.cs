using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;

namespace ForAdventure.Service
{
    public class RoomTypeService: IRoomTypeService
    {
        private ApplicationDbContext _db;
        public IRoomTypeRepository RoomType { get; private set; }
        public RoomTypeService(ApplicationDbContext db, IRoomTypeRepository roomType)
        {
            _db = db;
            RoomType = roomType;
        }

        public RoomType GetRoomTypeById(int? id)
        {
            RoomType roomType = RoomType.GetFirstOrDefault(u => u.Id == id);
            return roomType;
        }
    }
}

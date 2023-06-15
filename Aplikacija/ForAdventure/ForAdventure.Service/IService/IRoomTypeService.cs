using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IRoomTypeService
    {
        public IRoomTypeRepository RoomType { get; }
        public RoomType GetRoomTypeById(int? id);
    }
}

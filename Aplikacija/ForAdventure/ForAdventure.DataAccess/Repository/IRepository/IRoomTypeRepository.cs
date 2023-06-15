using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IRoomTypeRepository :IRepository<RoomType>
    {
        public void Save();
        void Update(RoomType roomType);
    }
}

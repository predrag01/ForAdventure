using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IRoomRepository :IRepository<Room>
    {
        public void Save();
        void Update(Room room);
    }
}

using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IHotelRepository :IRepository<Hotel>
    {
        public void Save();
        
        void Update(Hotel hotel);
    }
}

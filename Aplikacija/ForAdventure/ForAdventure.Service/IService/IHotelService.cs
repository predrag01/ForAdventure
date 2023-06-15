using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IHotelService
    {
        public IHotelRepository Hotel { get; }
        public IEnumerable<Hotel> GetHotelsByUser(string id);

        public Hotel GetHotelById(int? id);
        public string GetHotelImageJson(int? id);
        public bool DeletePicture(List<string>? image);
        void RemoveHotel(int id);
    }
}

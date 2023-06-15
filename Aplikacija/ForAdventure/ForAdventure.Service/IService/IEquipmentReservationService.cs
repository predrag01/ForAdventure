using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IEquipmentReservationService
    {
        public IEquipmentReservationRepository EquipmentReservation { get; }
        public IEnumerable<EquipmentReservation> GetUsersbyEquipment(int? id);
        public bool Check(int id, DateTime from, DateTime to);
        
    }
}

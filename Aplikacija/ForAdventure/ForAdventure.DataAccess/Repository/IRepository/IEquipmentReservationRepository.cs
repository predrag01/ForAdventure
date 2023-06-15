using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IEquipmentReservationRepository : IRepository<EquipmentReservation>
    {
        public void Save();
        void Update(EquipmentReservation reservation);
    }
}

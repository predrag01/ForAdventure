using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;


namespace ForAdventure.Service.IService
{
    public interface IEquipmentService
    {
        public IEquipmentRepository Equipment { get; }
        public IEnumerable<Equipment> GetEquipmentsByUser(string id);
        public Equipment GetEquipmentById(int? id);
        public bool DeletePicture(List<string> image);
        public IEnumerable<Equipment> GetEquipmentByEqType(int? id);
        void RemoveEquipment(int id);
    }
}

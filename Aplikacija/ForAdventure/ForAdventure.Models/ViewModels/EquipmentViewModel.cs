using ForAdventure.Models;

namespace ForAdventureWeb.Areas.Customer.ViewModels
{
    public class EquipmentsViewModel
    {
        public IEnumerable<Equipment> Equipments { get; set; }
        public IEnumerable<EquipmentType> EquipmentTypes { get; set; }
        public EquipmentsViewModel()
        {
        }
    }
}

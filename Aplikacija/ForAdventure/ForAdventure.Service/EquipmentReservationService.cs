using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ForAdventure.Service
{
    public class EquipmentReservationService : IEquipmentReservationService
    {
        private ApplicationDbContext _db;
        public IEquipmentReservationRepository EquipmentReservation { get; private set; }
        public EquipmentReservationService(ApplicationDbContext db, IEquipmentReservationRepository equipmentReservation)
        {
            _db = db;
            EquipmentReservation = equipmentReservation;
        }
        public IEnumerable<EquipmentReservation> GetUsersbyEquipment(int? id)
        {
            IEnumerable<EquipmentReservation> equipments = EquipmentReservation.WhereModified(x => x.EquipmentId == id);
            return equipments;
        }

		public bool Check(int id, DateTime from, DateTime to)
		{
			IEnumerable<EquipmentReservation> equipment= EquipmentReservation.GetAll().Where(u => u.EquipmentId== id);
			foreach (EquipmentReservation eq in equipment)
			{
				if ((from >= eq.ReservedFrom && from <= eq.ReservedUntil) || (to >= eq.ReservedFrom && to <= eq.ReservedUntil))
				{
					return false;
				}
			}
			return true;
		}
	}
}

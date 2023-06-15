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

namespace ForAdventure.Service
{
    public class RoomReservationService: IRoomReservationService
    {
        private ApplicationDbContext _db;
        public IRoomReservationRepository RoomReservation { get; private set; }

        public RoomReservationService(ApplicationDbContext db, IRoomReservationRepository roomReservation)
        {
            _db = db;
            RoomReservation = roomReservation;
        }

        public IEnumerable<RoomReservation> GetAllRoomReservationsByRoomId(int? id)
        {
            IEnumerable<RoomReservation> roomReservations = RoomReservation.WhereModified(u => u.RoomId == id);
            return roomReservations;
        }
		public bool Check(int id, DateTime from, DateTime to)
		{
			IEnumerable<RoomReservation> rooms= RoomReservation.GetAll().Where(u => u.RoomId == id);
			foreach (RoomReservation eq in rooms)
			{
				if (from >= eq.ReservedFrom && from <= eq.ReservedUntil || to >= eq.ReservedFrom && to <= eq.ReservedUntil)
				{
					return false;
				}
			}
			return true;
		}
	}
}

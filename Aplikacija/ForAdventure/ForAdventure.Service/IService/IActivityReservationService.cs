using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IActivityReservationService
	{
		public IActivityReservationRepository ActivityReservation { get; }
        public IEnumerable<ActivityReservation> GetUsersbyActivity(int? id);
        public bool Check(int id, int number, int capacity);
    }
}

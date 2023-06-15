using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IActivityReservationRepository:IRepository<ActivityReservation>
	{
		public void Save();
		void Update(ActivityReservation ActivityReservation);
	}
}

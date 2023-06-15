using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository.IRepository
{
    public interface IReportRepository:IRepository<Report>
	{
		void Update(Report report);
		public void Save();
	}
}

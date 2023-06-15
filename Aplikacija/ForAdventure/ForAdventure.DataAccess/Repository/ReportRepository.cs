using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.DataAccess.Repository
{
    public class ReportRepository:Repository<Report>, IReportRepository
	{
		private ApplicationDbContext _db;
        public ReportRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

		public void Save()
		{
			_db.SaveChanges();
		}

		public void Update(Report report)
		{
			_db.Reports.Update(report);
		}
	}
}

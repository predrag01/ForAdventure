using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;

namespace ForAdventure.Service.IService
{
    public interface IReportService
	{
		public IReportRepository Report { get; }

		int GetNumberOfReports(string id);
		IEnumerable<Report> GetAllUnchecked();
        IEnumerable<ApplicationUser> GetUnchecked();
        IEnumerable<ApplicationUser> GetReportedUsers();
		IEnumerable<ApplicationUser> GetChecked();

		IEnumerable<Report> GetReportsForUser(string userId);
		void Check(int id);
		void RemoveReports(string id);

    }
}

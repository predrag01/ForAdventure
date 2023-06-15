using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;

namespace ForAdventure.Service
{
    public class ReportService : IReportService
	{
		private ApplicationDbContext _db;
		public IReportRepository Report { get; private set; }
		private IApplicationUserService _userService;
        public ReportService(ApplicationDbContext db, IApplicationUserService ser, IReportRepository report)
        {
			_db = db;
			Report = report;
			_userService = ser;
		}

		public int GetNumberOfReports(string id)
		{
			var list=Report.GetAll().Where(x=>x.ReportedId==id).ToList();
			return list.Count;
		}
        public IEnumerable<Report> GetAllUnchecked()
        {
            IEnumerable<Report> list = Report.GetAll().Where(x => x.Checked == false);
            return list;
        }

        public IEnumerable<ApplicationUser> GetUnchecked()
		{
            IList<ApplicationUser> list = new List<ApplicationUser>();
            IEnumerable<Report> reports = Report.GetAll().Where(x => x.Checked == false);
            foreach (Report report in reports)
            {
                if (!list.Contains(_userService.GetUserById(report.ReportedId)))
                {
                    list.Add(_userService.GetUserById(report.ReportedId));
                }
            }
            return list;
        }
        public IEnumerable<ApplicationUser> GetChecked()
        {
            IList<ApplicationUser> list = new List<ApplicationUser>();
            IEnumerable<Report> reports = Report.GetAll().Where(x => x.Checked == true);
            foreach (Report report in reports)
            {
                if (!list.Contains(_userService.GetUserById(report.ReportedId)))
                {
                    list.Add(_userService.GetUserById(report.ReportedId));
                }
            }
            return list;
        }

        public IEnumerable<ApplicationUser> GetReportedUsers()
        {
            IList<ApplicationUser>list=new List<ApplicationUser>();
			IEnumerable<Report> reports = Report.GetAll();
			foreach (Report report in reports)
			{
				if (!list.Contains(_userService.GetUserById(report.ReportedId)))
				{
					list.Add(_userService.GetUserById(report.ReportedId));
				}
			}
			return list;
        }

        public IEnumerable<Report> GetReportsForUser(string userId)
        {
            IEnumerable<Report> reports=Report.GetAll().Where(x=>x.ReportedId==userId);
            return reports;
        }

        public void Check(int id)
        {
            Report report = Report.GetFirstOrDefault(x => x.Id == id);
            report.Checked = true;
        }

        public void RemoveReports(string id)
        {
            var reports = Report.GetAll().Where(x => x.ReporterId == id);
            if (reports != null)
            {
                Report.RemoveRange(reports);
                Report.Save();
            }
            var reported = Report.GetAll().Where(x => x.ReportedId == id);
            if (reports != null)
            {
                Report.RemoveRange(reported);
                Report.Save();
            }
        }
    }
}

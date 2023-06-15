using ForAdventure.Models;
using ForAdventure.Service.IService;
using ForAdventure.Service.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Composition;

namespace ForAdventureWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class ReportController : Controller
	{
		private readonly IReportService _service;
        public ReportController(IReportService service)
        {
			_service = service;
        }
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult Index()
		{
			IEnumerable<Report> reports = _service.GetAllUnchecked();
			return View(reports);
		}
		//GET - create report
		public IActionResult Report(string reporterId, string ReportedId) 
		{
			Report report=new Report();
			report.ReporterId = reporterId;
			report.ReportedId = ReportedId;
			return View(report);
		}

		[HttpPost]
		public IActionResult Report(Report report)
		{
			if(ModelState.IsValid)
			{
				report.Checked = false;

				_service.Report.Add(report);
				_service.Report.Save();
				return RedirectToAction("Profile", "ApplicationUser", new { id = report.ReportedId });
			}
			return View(report);
		}

		[HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult Check(int id)
		{
			if(id == 0)
			{
				return NotFound();
			}
			_service.Check(id);
			_service.Report.Save();
			Report report=_service.Report.GetFirstOrDefault(x => x.Id == id);
            return RedirectToAction("Profile", "ApplicationUser", new { id = report.ReportedId });
        }
	}
}

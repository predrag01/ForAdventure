using ForAdventure.Models;
using ForAdventure.Service.IService;
using ForAdventure.Service.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForAdventureWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.AdminRole)]
	public class ActivityTypeController : Controller
	{
		private readonly IActivityTypeService _service;

		public ActivityTypeController(IActivityTypeService service)
		{
			_service = service;
		}
		public IActionResult Index()
		{
			IEnumerable<ActivityType> objActivityTypeList = _service.ActivityType.GetAll();
			return View(objActivityTypeList);
		}
		//GET
		public IActionResult Create()
		{
			return View();
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(ActivityType obj)
		{
			_service.ActivityType.Add(obj);
			_service.ActivityType.Save();
			return RedirectToAction("Index");
		}

		//GET
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var obj = _service.ActivityType.GetFirstOrDefault(u => u.Id == id);
			if (obj == null)
				return NotFound();
			return View(obj);
		}

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _service.ActivityType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _service.ActivityType.Remove(obj);
            _service.ActivityType.Save();
           return Json(new { success = true, redirectUrl = Url.Action("Index", "ActivityType") });
        }
    }
}
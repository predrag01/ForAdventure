using ForAdventure.Models;
using ForAdventure.Service.IService;
using ForAdventure.Service.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForAdventureWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class EquipmentTypeController : Controller
    {
        private readonly IEquipmentTypeService _service;

        public EquipmentTypeController(IEquipmentTypeService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            IEnumerable<EquipmentType> objEquipmentTypeList = _service.EquipmentType.GetAll();
            return View(objEquipmentTypeList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EquipmentType obj)
        {
            _service.EquipmentType.Add(obj);
            _service.EquipmentType.Save();
            return RedirectToAction("Index");
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _service.EquipmentType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
                return NotFound();
            return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EquipmentType obj)
        {
            _service.EquipmentType.Update(obj);
            _service.EquipmentType.Save();
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _service.EquipmentType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _service.EquipmentType.Remove(obj);
            _service.EquipmentType.Save();
            return Json(new { success = true, redirectUrl = Url.Action("Index", "EquipmentType") });
        }
       
    }
}

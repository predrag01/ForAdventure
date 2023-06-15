using ForAdventure.Models;
using ForAdventure.Service;
using ForAdventure.Service.IService;
using ForAdventure.Service.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForAdventureWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeService _service;
        private readonly IRoomService _roomService;

        public RoomTypeController(IRoomTypeService service, IRoomService roomService)
        {
            _service = service;
            _roomService = roomService;
        }
        public IActionResult Index()
        {
            IEnumerable<RoomType> objRoomTypeList = _service.RoomType.GetAll();
            return View(objRoomTypeList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomType obj)
        {
            _service.RoomType.Add(obj);
            _service.RoomType.Save();
            return RedirectToAction("Index");
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _service.RoomType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
                return NotFound();
            return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoomType obj)
        {
            _service.RoomType.Update(obj);
            _service.RoomType.Save();
            return RedirectToAction("Index");
        }

        //DELETE
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int? id)
        {
            var obj = _service.RoomType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            //_service.(id.Value);
            var rooms = _roomService.Room.GetAll().Where(r => r.RoomTypeId == id);
            foreach(var room in rooms)
            {
                _roomService.RemoveRoom(room.Id);
            }
            _service.RoomType.Remove(obj);
            _service.RoomType.Save();
            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

        ////GET
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var obj = _service.RoomType.GetFirstOrDefault(u => u.Id == id);
        //    if (obj == null)
        //        return NotFound();
        //    return View(obj);
        //}

        ////POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePOST(int? id)
        //{
        //    var obj = _service.RoomType.GetFirstOrDefault(u => u.Id == id);
        //    _service.RoomType.Remove(obj);
        //    _service.RoomType.Save();
        //    return RedirectToAction("Index");
        //}
    }
}

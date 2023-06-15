using ForAdventure.Models;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ForAdventureWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class RoomController : Controller
    {
        private readonly IRoomService _service;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IHotelService _hotelService;
        private readonly IShoppingCartService _shoppingService;

        public RoomController(IRoomService service, IRoomTypeService roomTypeService, IHotelService hotelService)
        {
            _service = service;
            _roomTypeService = roomTypeService;
            _hotelService = hotelService;
        }
        public IActionResult Index(int? HotelId)
        {
            IEnumerable<Room> objRoomList = _service.GetAllRoomsByHotelId(HotelId);
            ViewBag.HotelName = _hotelService.GetHotelById(HotelId).Name;
            ViewBag.HotelId = HotelId;
            return View(objRoomList);
        }
        //GET
        [Authorize]
        public IActionResult Create(int? HotelId)
        {
            var roomTypes = _roomTypeService.RoomType.GetAll();
            var roomTypeList = new SelectList(roomTypes, "Id", "Type");
            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.hotelID = HotelId;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Room obj)
        {
            _service.Room.Add(obj);
            _service.Room.Save();
            return RedirectToAction("MyHotels", "Hotel");
        }

        //GET
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var obj=_service.Room.GetFirstOrDefault(u=>u.Id==id);
            if (obj == null)
                return NotFound();
            return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Room obj)
        {
            _service.Room.Update(obj);
            _service.Room.Save();
            return RedirectToAction("Index", new { hotelId = obj.HotelId });
        }

        //DELETE
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int? id)
        {
            var obj = _service.Room.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _service.RemoveRoom(id.Value);
            _service.Room.Remove(obj);
            _service.Room.Save();
            return Json(new { success = true, redirectUrl = Url.Action("Index", new { HotelId = obj.HotelId }) });
        }
    }
}

using ForAdventure.Models;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using ForAdventure.Service.Utility;

namespace ForAdventureWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HotelController : Controller
    {
        private readonly IHotelService _service;
        private readonly IRoomService _roomService;
        private readonly IShoppingCartService _cartService;
        private readonly IApplicationUserService _userService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HotelController(IHotelService service, IWebHostEnvironment hostEnvironment, IRoomService roomService, IShoppingCartService ser, IApplicationUserService userService)
        {
            _service = service;
            _hostEnvironment = hostEnvironment;
            _roomService = roomService;
            _cartService = ser;
            _userService = userService;
        }
        public IActionResult Index()
        {
            IEnumerable<Hotel> objHotelList = _service.Hotel.GetAll().Reverse();
            objHotelList = objHotelList.Where(h => _roomService.GetAllRoomsByHotelId(h.Id).Any()).ToList();
            objHotelList=objHotelList.Take(5);
            return View(objHotelList);
        }
        [Authorize]
        public IActionResult MyHotels()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<Hotel> objHotelList = _service.GetHotelsByUser(claim.Value);
            objHotelList = objHotelList.Reverse();
            return View(objHotelList);
        }
        public IActionResult HotelsList(string sortingOption, bool? poolState, bool? gymState, bool? spaState, bool? saunaState, bool? restaurantState, bool? parkingState, int page)
        {
            IEnumerable<Hotel> objHotelList = _service.Hotel.GetAll();

            if (poolState.HasValue && poolState.Value)
            {
                objHotelList = objHotelList.Where(hotel => hotel.Pool == true);
            }

            if (gymState.HasValue && gymState.Value)
            {
                objHotelList = objHotelList.Where(hotel => hotel.Gym == true);
            }

            if (spaState.HasValue && spaState.Value)
            {
                objHotelList = objHotelList.Where(hotel => hotel.Spa == true);
            }

            if (saunaState.HasValue && saunaState.Value)
            {
                objHotelList = objHotelList.Where(hotel => hotel.Sauna == true);
            }

            if (restaurantState.HasValue && restaurantState.Value)
            {
                objHotelList = objHotelList.Where(hotel => hotel.Restaurant == true);
            }

            if (parkingState.HasValue && parkingState.Value)
            {
                objHotelList = objHotelList.Where(hotel => hotel.Parking == true);
            }

            foreach (Hotel hotel in objHotelList)
            {
                hotel.Rooms = _roomService.GetAllRoomsByHotelId(hotel.Id).ToList();
            }

            objHotelList = objHotelList.Where(h => _roomService.GetAllRoomsByHotelId(h.Id).Any()).ToList();

            switch (sortingOption)
            {
                case SD.FetchHotels:
                    objHotelList = objHotelList.Reverse();
                    break;
                case SD.HighToLow:
                    objHotelList = objHotelList.OrderByDescending(hotel => hotel.Rooms.Min(room => _roomService.GetLowestPrice(hotel.Id)));
                    break;
                case SD.LowToHigh:
                    objHotelList = objHotelList.OrderBy(hotel => hotel.Rooms.Min(room => _roomService.GetLowestPrice(hotel.Id)));
                    break;
                case SD.Newest:
                    objHotelList = objHotelList.Reverse();
                    break;
                case SD.Oldest:
                    break;
                default:
                    objHotelList = objHotelList.Reverse();
                    break;
            }

            objHotelList = objHotelList.Skip((page - 1) * 5).Take(5);

            return PartialView("_HotelList", objHotelList);
        }
        //GET
        [Authorize]
        public IActionResult Create(string? OwnerId)
        {
            ViewBag.OwnerId = OwnerId;
            return View();
        }

        //POST
        //https://localhost:7108/Customer/Hotel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Hotel obj, List<IFormFile> files)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            obj.OwnerId = claim.Value;
			if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                List<string> imageUrls = new();
                foreach(var file in files)
                {
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"images\hotels");
                        var extension = Path.GetExtension(file.FileName);
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }
                        imageUrls.Add(@"\images\hotels\" + fileName + extension);
                    }
                }
                obj.ImageURLsJson = JsonSerializer.Serialize(imageUrls);
                _service.Hotel.Add(obj);
                _service.Hotel.Save();
                return RedirectToAction("MyHotels");
            }
            return View(obj);
        }

        //GET
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var obj=_service.Hotel.GetFirstOrDefault(u=>u.Id==id);
            if (obj == null)
                return NotFound();
            return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Hotel obj, List<IFormFile>? files)
        {
			if (ModelState.IsValid)
			{
                if (files != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    List<string> imageUrls = new();
                    foreach (var file in files)
                    {
                        if (file != null)
                        {
                            string fileName = Guid.NewGuid().ToString();
                            var uploads = Path.Combine(wwwRootPath, @"images\hotels");
                            var extension = Path.GetExtension(file.FileName);
                            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                            }
                            imageUrls.Add(@"\images\hotels\" + fileName + extension);
                        }
                    }
                    obj.ImageURLsJson = JsonSerializer.Serialize(imageUrls);
                }

                var existingHotel = _service.Hotel.GetFirstOrDefault(u => u.Id == obj.Id);

				existingHotel.Name = obj.Name;
				existingHotel.Location = obj.Location;
				existingHotel.Address = obj.Address;
				existingHotel.Phone = obj.Phone;
				existingHotel.Email = obj.Email;
				existingHotel.Pool = obj.Pool;
				existingHotel.Gym = obj.Gym;
				existingHotel.Spa = obj.Spa;
				existingHotel.Sauna = obj.Sauna;
				existingHotel.Restaurant = obj.Restaurant;
                if (files.Count!=0)
                {
                    existingHotel.ImageURLsJson = obj.ImageURLsJson;
                }
				existingHotel.Parking = obj.Parking;
				existingHotel.Rooms = obj.Rooms;
				existingHotel.OwnerId = obj.OwnerId;
				existingHotel.Owner = obj.Owner;

				_service.Hotel.Update(existingHotel);
				_service.Hotel.Save();
				return RedirectToAction("MyHotels");
			}
			return View(obj);
        }

        //DELETE
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int? id)
        {
            var obj = _service.Hotel.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _service.RemoveHotel(id.Value);
            _service.DeletePicture(obj.ImageURLs);
            _service.Hotel.Remove(obj);
            _service.Hotel.Save();
            return Json(new { success = true, redirectUrl = Url.Action("MyHotels", "Hotel") });
        }

        //GET
        public IActionResult HotelPage(int HotelId)
        {
            ShoppingCart cart = new ShoppingCart();
            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (HotelId == null || HotelId == 0)
                {
                    return NotFound();
                }
                
                if (_cartService.HotelInCart(HotelId))
                {
                    cart = _cartService.GetCartbyHotelId(HotelId);
                }
                else if (_cartService.IsUserInCart(claim.Value))
                {
                    cart = _cartService.GetHotelCartByUser(claim.Value);
                }
            }
             
            if (cart == null)
            {
                cart = new ShoppingCart();
            }

            cart.Hotel = _service.GetHotelById(HotelId);
            cart.Hotel.Rooms = _roomService.GetAllRoomsByHotelId(HotelId).ToList();
            cart.Hotel.Owner = _userService.GetUserById(cart.Hotel.OwnerId);
            cart.Room = cart.Hotel.Rooms.FirstOrDefault();

            if (cart.Hotel == null)
                return NotFound();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult HotelPage(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.ApplicationUserId = claim.Value;

            if (cart.Room.Id == null)
                return NotFound();
            cart.RoomId = cart.Room.Id;
            cart.Room = null;
			if (cart.Id != null)
			{
				_cartService.ShoppingCart.Update(cart);
			}
			else
			{
				_cartService.ShoppingCart.Add(cart);
			}
			_cartService.ShoppingCart.Save();
            return RedirectToAction("Index");
        }
    }
}

using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Hosting;

namespace ForAdventure.Service
{
    public class HotelService:IHotelService
    {
        private ApplicationDbContext _db;
        public IHotelRepository Hotel{ get; private set; }
        public RoomService roomService;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IShoppingCartService _cartService;
        private readonly IRoomService _roomService;
        private readonly IRoomReservationService _ReservationService;
        public HotelService(ApplicationDbContext db, IWebHostEnvironment webHostEnviroment, IShoppingCartService cartService, IRoomService roomService,
            IRoomReservationService reservationService, IHotelRepository hotel)
        {
            _db = db;
            Hotel = hotel;
            _webHostEnviroment = webHostEnviroment;
            _cartService = cartService;
            _roomService = roomService;
            _ReservationService = reservationService;
        }
        public IEnumerable<Hotel> GetHotelsByUser(string id)
        {
            IEnumerable<Hotel> hotels = Hotel.WhereModified(u => u.OwnerId == id);
            return hotels;
        }
        public Hotel GetHotelById(int? id)
        {
            Hotel hotel = Hotel.GetFirstOrDefault(u=>u.Id == id);
            return hotel;
        }

		public string GetHotelImageJson(int? id)
		{
			Hotel hotel = Hotel.GetFirstOrDefault(u => u.Id == id);
            return hotel.ImageURLsJson;
		}
        public bool DeletePicture(List<string>? images)
        {
            string wwwRootPath = _webHostEnviroment.WebRootPath;
            foreach(string image in images)
            {
                if (image != null)
                {
                    var oldImage = Path.Combine(wwwRootPath, image.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                        return true;
                    }
                }
            }
                return false;
        }

        public void RemoveHotel(int id)
        {
            var rooms=_roomService.GetAllRoomsByHotelId(id);
            if(rooms!= null)
            {
                foreach(var room in rooms)
                {
                    var cart = _cartService.ShoppingCart.GetAll().Where(u=>u.RoomId==room.Id);
                    foreach(var cartItem in cart)
                    {
                        if (cartItem.ActivityId == null && cartItem.EquipmentId == null)
                        {
                            _cartService.ShoppingCart.Remove(cartItem);
                        }
                        else
                        {
                            cartItem.RoomId = null;
                            cartItem.Room = null;
                            cartItem.DateFromRoom = null;
                            cartItem.DateToRoom = null;
                        }
                        _cartService.ShoppingCart.Save();
                    }
                    var res = _ReservationService.RoomReservation.GetAll().Where(u => u.RoomId == room.Id);
                    if(res!= null)
                    {
                        _ReservationService.RoomReservation.RemoveRange(res);
                        _ReservationService.RoomReservation.Save();
                    }
                }
            }
        }
    }
}

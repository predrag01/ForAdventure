using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;

namespace ForAdventure.Service
{
    public class RoomService:IRoomService
    {
        private ApplicationDbContext _db;
        public IRoomRepository Room{ get; private set; }
        private readonly IShoppingCartService _cartService;
        private readonly IRoomReservationService _ReservationService;
        public RoomService(ApplicationDbContext db, IShoppingCartService cartService, IRoomReservationService ReservationService, IRoomRepository room)
        {
            _db = db;
            Room = room;
            _cartService= cartService;
            _ReservationService= ReservationService;
        }

        public IEnumerable<Room> GetAllRoomsByHotelId(int? id)
        {
            IEnumerable<Room> rooms = Room.WhereModified(u => u.HotelId == id);
            return rooms;
        }
        public float GetLowestPrice(int? id)
        {
            IEnumerable<Room> rooms = GetAllRoomsByHotelId(id);
            if (rooms == null)
            {
                return 0;
            }
            else
            {
                float lowest = rooms.First().PricePerNight;
                foreach(Room room in rooms)
                {
                    if (lowest > room.PricePerNight)
                        lowest = room.PricePerNight;
                }
                if (lowest > 0)
                {
                    return lowest;
                }
                return 0;
            }
        }
        public int RoomCount(int? id)
        {
            IEnumerable<Room> rooms = GetAllRoomsByHotelId(id);
            int count = rooms.Count();
            return count;
        }

		public Room GetRoomById(int? id)
		{
			return Room.GetFirstOrDefault(u => u.Id == id);
		}

        public void RemoveRoom(int id)
        {
            var cart = _cartService.ShoppingCart.GetAll().Where(u => u.RoomId == id);
            foreach (var cartItem in cart)
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
            var res = _ReservationService.RoomReservation.GetAll().Where(u => u.RoomId == id);
            if (res != null)
            {
                _ReservationService.RoomReservation.RemoveRange(res);
                _ReservationService.RoomReservation.Save();
            }
        }
    }
}

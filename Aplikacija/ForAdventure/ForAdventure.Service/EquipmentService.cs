using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Hosting;

namespace ForAdventure.Service
{
    public class EquipmentService : IEquipmentService
    {
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IShoppingCartService _cartService;
        private readonly IEquipmentReservationService _ReservationService;
        public IEquipmentRepository Equipment { get; private set; }
        public EquipmentService(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IShoppingCartService cartService, IEquipmentReservationService reservationService, IEquipmentRepository equipment)
        {
            _db = db;
            Equipment = equipment;
            _webHostEnviroment = webHostEnvironment;
            _cartService = cartService;
            _ReservationService = reservationService;
        }
        public IEnumerable<Equipment> GetEquipmentsByUser(string id)
        {
            IEnumerable<Equipment> equipments = Equipment.WhereModified(u => u.OwnerId == id);
            return equipments;
        }
        public Equipment GetEquipmentById(int? id)
        {
            Equipment equipment = Equipment.GetFirstOrDefault(u => u.ID == id);
            return equipment;
        }
        public IEnumerable<Equipment> GetEquipmentByEqType(int? id)
        {
            IEnumerable<Equipment> list = Equipment.WhereModified(u=>u.EqTypeId == id);
            return list;
            
        }
        public bool DeletePicture(List<string> image)
        {
            string wwwRootPath = _webHostEnviroment.WebRootPath;
            if (image != null)
            {
                foreach (string path in image) {
                    var oldImage = Path.Combine(wwwRootPath, path.TrimStart('\\'));
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                        
                    }
                }
                
               
            }
            return false;
        }

        public void RemoveEquipment(int id)
        {
            var cart = _cartService.ShoppingCart.GetAll().Where(u => u.EquipmentId == id);
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    if (item.ActivityId == null && item.RoomId == null)
                    {
                        _cartService.ShoppingCart.Remove(item);
                    }
                    else
                    {
                        item.EquipmentId = null;
                        item.Equipment = null;
                        item.DateFromEquipment = null;
                        item.DateToEquipment = null;
                    }
                    _cartService.ShoppingCart.Save();

                }
            }

            var res = _ReservationService.EquipmentReservation.GetAll().Where(u => u.EquipmentId == id);
            if (res != null)
            {
                foreach (var item in res)
                {
                    _ReservationService.EquipmentReservation.Remove(item);
                    _ReservationService.EquipmentReservation.Save();

                }
            }
        }
    }
}

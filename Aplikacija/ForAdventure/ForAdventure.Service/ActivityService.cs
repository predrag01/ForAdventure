using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Hosting;

namespace ForAdventure.Service
{
    public class ActivityService : IActivityService
    {
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IShoppingCartService _cartService;
        private readonly IActivityReservationService _ReservationService;
        public IActivityRepository Activity {get; private set;}

        public ActivityService(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IShoppingCartService ser, IActivityReservationService ReservationService, IActivityRepository activity)
        {
            _db = db;
            Activity= activity;
            _webHostEnviroment = webHostEnvironment;
            _cartService = ser;
            _ReservationService = ReservationService;
        }
		public IEnumerable<Activity> GetActivitiesByUser(string id)
		{
			IEnumerable<Activity> activities = Activity.WhereModified(u => u.UserId == id);
			return activities;
		}
        public Activity GetActivityById(int? id)
        {
            Activity activity = Activity.GetFirstOrDefault(u => u.Id == id);
            return activity;
        }

        public IEnumerable<Activity> GetActivityByActivityType(int? id)
        {
            IEnumerable<Activity> list = Activity.WhereModified(u => u.ActivityTypeId == id);
            return list;

        }

        public bool DeletePicture(string image)
        {
            string wwwRootPath = _webHostEnviroment.WebRootPath;
            if (image != null)
            {
                var oldImage = Path.Combine(wwwRootPath, image.TrimStart('\\'));
                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                    return true;
                }
            }
            return false;
        }

        public void RemoveActivity(int id)
        {
            var cart=_cartService.ShoppingCart.GetAll().Where(u=>u.ActivityId==id);
            if(cart!=null)
            {
                foreach (var item in cart)
                {
                    if (item.EquipmentId == null && item.RoomId == null)
                    {
                        _cartService.ShoppingCart.Remove(item);
                    }
                    else
                    {
                        item.Activity = null;
                        item.ActivityId = null;
                        item.NumberOfPersons = null;
                    }
                    _cartService.ShoppingCart.Save();

                }
            }
            
            var res=_ReservationService.ActivityReservation.GetAll().Where(u => u.ActivityId == id);
            if (res != null)
            {
                foreach (var item in res)
                {
                    _ReservationService.ActivityReservation.Remove(item);
                    _ReservationService.ActivityReservation.Save();

                }
            }
        }
    }
}
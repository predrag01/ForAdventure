using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ForAdventure.Service
{
    public class ApplicationUserService : IApplicationUserService
    {
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActivityService _activityService;
        private readonly IEquipmentService _equipmentService;
        private readonly IHotelService _hotelService;

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public ApplicationUserService(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,
            IActivityService activityService, IEquipmentService equipmentService, IHotelService hotelService, IApplicationUserRepository applicationUser)
        {
            _db = db;
            ApplicationUser = applicationUser;
            _webHostEnviroment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _activityService = activityService;
            _equipmentService = equipmentService;
            _hotelService = hotelService;
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public string GetImageURLByEmail(string email)
        {
            ApplicationUser user = ApplicationUser.GetFirstOrDefault(u => u.Email == email);
            string image = null;
            if (user != null)
            {
                image = user.ImageURL;
            }
            return (image);
        }
        public string GetUsernameByEmail(string email)
        {
            ApplicationUser user = ApplicationUser.GetFirstOrDefault(u => u.Email == email);
            string image = null;
            if (user != null)
            {
                image = user.NameInApplication;
            }
            return (image);
        }

        public ApplicationUser GetUserById(string id)
        {
            ApplicationUser user = ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            return user;
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
        public string? GetIdByEmail(string email)
        {
            string id=null;
            ApplicationUser applicationUser = ApplicationUser.GetFirstOrDefault(u => u.Email == email);
            if (applicationUser != null)
            {
                id = applicationUser.Id;
            }
            return id;
        }

		public string GetProfilePictureById(string id)
		{
			ApplicationUser user= ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            string image = null;
            if(user != null)
            {
                image = user.ImageURL;
            }
            return image;
		}

        public string GetUsernameById(string id)
        {
            ApplicationUser user = ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            string image = null;
            if (user != null)
            {
                image = user.NameInApplication;
            }
            return image;
        }

        public string GetEmailByUserId(string id)
        {
            string email=ApplicationUser.GetFirstOrDefault(x=>x.Id == id).Email;
            return email;
        }

        public void RemoveUser(string id)
        {
            var activity=_activityService.GetActivitiesByUser(id);
            var equipment = _equipmentService.GetEquipmentsByUser(id);
            var hotels = _hotelService.GetHotelsByUser(id);
            if(activity != null) 
            {
                foreach (var obj in activity)
                {
                    _activityService.RemoveActivity(obj.Id);
                    _activityService.Activity.Remove(obj);
                    _activityService.Activity.Save();
                }
            }
            if (equipment != null)
            {
                foreach (var obj in equipment)
                {
                    _equipmentService.RemoveEquipment(obj.ID);
                    _equipmentService.Equipment.Remove(obj);
                    _equipmentService.Equipment.Save();
                }
            }
            if (hotels != null)
            {
                foreach (var obj in hotels)
                {
                    _hotelService.RemoveHotel(obj.Id);
                    _hotelService.Hotel.Remove(obj);
                    _hotelService.Hotel.Save();
                }
            }
            
        }
    }
}
using ForAdventure.Models;
using Microsoft.AspNetCore.Mvc;
using ForAdventure.Service.IService;
using ForAdventure.Models.ViewModels;

namespace ForAdventureWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IActivityService _service;
        private readonly IActivityService _activityService;
        private readonly IEquipmentService _equipmentService;
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;

        public HomeController(ILogger<HomeController> logger, IActivityService service, IActivityService activityService, IEquipmentService equipmentService, IHotelService hotelService, IRoomService roomService)
        {
            _logger = logger;
            _service = service;
            _activityService = activityService;
            _equipmentService = equipmentService;
            _hotelService = hotelService;
            _roomService = roomService;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.LatestActivities = _activityService.Activity.GetAll().Take(5);
            homeVM.LatestActivities = homeVM.LatestActivities.Reverse();
            homeVM.LatestEquipment = _equipmentService.Equipment.GetAll().Take(5);
            homeVM.LatestEquipment = homeVM.LatestEquipment.Reverse();
            homeVM.LatestHotels = _hotelService.Hotel.GetAll();
            homeVM.LatestHotels = homeVM.LatestHotels.Reverse();
            homeVM.LatestHotels = homeVM.LatestHotels.Where(h => _roomService.GetAllRoomsByHotelId(h.Id).Any()).Take(5).ToList();
            return View(homeVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
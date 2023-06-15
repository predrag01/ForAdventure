using ForAdventure.Models;
using ForAdventure.Models.ViewModels;
using ForAdventure.Service.IService;
using ForAdventure.Service.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ForAdventureWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationUserController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IApplicationUserService _service;
		private readonly IActivityService _serviceActivity;
		private readonly IHotelService _serviceHotel;
        private readonly IReportService _serviceReport;
        private readonly IEquipmentService _equipment;
        private readonly IReportService _reportService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public ProfileVM profile { get; set; }

		public ApplicationUserController(IApplicationUserService service, IWebHostEnvironment webHostEnvironment,
            RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IActivityService serviceActivity,
            IReportService ser, IHotelService serviceHotel, IEquipmentService eq, IReportService reportService)
		{
			_service = service;
			_webHostEnviroment = webHostEnvironment;
			_roleManager = roleManager;
			_userManager = userManager;
			_serviceActivity = serviceActivity;
			_serviceHotel = serviceHotel;
            _serviceReport = ser;
            _equipment = eq;
            _reportService = reportService;
		}
        // GET: ApplicationUserController
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult Index()
        {
            IEnumerable<ApplicationUser> users = _service.ApplicationUser.GetAll();
            foreach(ApplicationUser app in users)
            {
                var roles = _userManager.GetRolesAsync(app).Result;
                if(roles.FirstOrDefault()==SD.AdminRole)
                {
                    app.Role = SD.AdminRole;
                }
                else
                {
                    app.Role = SD.UserRole;
                }
                app.ReportsCount = _serviceReport.GetNumberOfReports(app.Id);
            }
            users = users.Take(5);
            return View(users);
        }
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult FilterUsers(string filter, int page)
        {
            IEnumerable<ApplicationUser> users = _service.ApplicationUser.GetAll(); ;
            switch (filter)
            {
                case "reported":
                    users = null;
                    users = _serviceReport.GetReportedUsers();
                    break;
                case "checked":
                    users = null;
                    users = _serviceReport.GetChecked();
                    break;
                case "unchecked":
                    users = null;
                    users = _serviceReport.GetUnchecked();
                    break;
            }
            foreach (ApplicationUser app in users)
            {
                var roles = _userManager.GetRolesAsync(app).Result;
                if (roles.FirstOrDefault() == SD.AdminRole)
                {
                    app.Role = SD.AdminRole;
                }
                else
                {
                    app.Role = SD.UserRole;
                }
                app.ReportsCount = _serviceReport.GetNumberOfReports(app.Id);
            }
            users = users.Skip((page - 1) * 5).Take(5);
            return PartialView("_UsersList", users);
        }

        [HttpDelete]
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult Delete(string? id)
        {
            var obj = _service.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _service.RemoveUser(id);
            _service.DeletePicture(obj.ImageURL);
            _reportService.RemoveReports(id);
            _service.ApplicationUser.Remove(obj);
            _service.ApplicationUser.Save();
            return Json(new { success = true, redirectUrl = Url.Action("Index", "ApplicationUser") });
        }

        //GET
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UserVM user = new UserVM();
            user.User = _service.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            if (user.User == null)
            {
                return NotFound();
            }
            var role = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem()
            {
                Text = i,
                Value = i
            });
            ViewBag.role = role;
            user.Reports = _serviceReport.GetReportsForUser(id);
            user.Count = _serviceReport.GetNumberOfReports(id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.AdminRole)]
        public async Task<IActionResult> Edit(ApplicationUser obj, IFormFile? file)
        {
            if(obj.Id!=null)
            {
                ApplicationUser user=_service.ApplicationUser.GetFirstOrDefault(x=>x.Id==obj.Id);
                if (obj.Role != null)
                {
                    var roles = _userManager.GetRolesAsync(user).Result;
                    if (roles.FirstOrDefault() == SD.AdminRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, SD.AdminRole);
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, SD.UserRole);
                    }
                    await _userManager.AddToRoleAsync(user, obj.Role);
                    return RedirectToAction("Index");
                }
            }
            return View(obj);
        }

        //GET
        public IActionResult Profile(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _service.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

			profile = new ProfileVM();
			profile.User=obj;
            profile.ActivityList = _serviceActivity.GetActivitiesByUser(id);
            return View(profile);
        }

		[HttpGet]
		public IActionResult UserAdvertisement(string? filter, string userId)
        {
            profile=new ProfileVM();
            if(filter== "equipment")
            {
                profile.EquipmentList = _equipment.GetEquipmentsByUser(userId);
                if (profile.EquipmentList.ToList().Count == 0)
                {
                    profile.HotelList = null;
                }
                return PartialView("_UserAdvertisement", profile);
			}
            else if(filter=="hotel")
            {
                profile.HotelList= _serviceHotel.GetHotelsByUser(userId);
                if(profile.HotelList.ToList().Count==0)
                {
                    profile.HotelList = null;
                }
                return PartialView("_UserAdvertisement", profile);
            }
            else
            {
                profile.ActivityList= _serviceActivity.GetActivitiesByUser(userId);
				if (profile.ActivityList.ToList().Count == 0)
				{
					profile.ActivityList = null;
				}
				return PartialView("_UserAdvertisement", profile);
			}
        }
    }

}

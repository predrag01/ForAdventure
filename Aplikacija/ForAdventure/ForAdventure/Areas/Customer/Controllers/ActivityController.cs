using Microsoft.AspNetCore.Mvc;
using ForAdventure.Models;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ForAdventureWeb.Areas.Customer.ViewModels;
using ForAdventure.Service.Utility;

namespace ForAdventureWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ActivityController : Controller
    {

        private readonly IActivityService _service;
        private readonly IActivityTypeService _activityTypeService;
		private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IShoppingCartService _cartService;
        private readonly IApplicationUserService _userService;

        public ActivityController(IActivityService service, IActivityTypeService activityTypeService, IWebHostEnvironment hostEnvironment, IShoppingCartService ser, IApplicationUserService applicationUserService)
        {
            _service = service;
            _activityTypeService = activityTypeService;
            _hostEnvironment = hostEnvironment;
            _cartService= ser;
            _userService = applicationUserService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Activity> activity = _service.Activity.GetAll().Where(u=>u.DateFrom>=DateTime.Now).Reverse();
            IEnumerable<ActivityType> types = _activityTypeService.ActivityType.GetAll();
           
            ActivitiesViewModel viewModel = new ActivitiesViewModel()
            {
                Activities = activity.Take(5),
               
                ActivityTypes = types
            };
           
            return View(viewModel);
        }

        [Authorize]
        public IActionResult MyActivities(string UserId)
        {
            IEnumerable<Activity> objActivityList = _service.GetActivitiesByUser(UserId);
            objActivityList = objActivityList.Reverse();
            return View(objActivityList);
        }

        [HttpGet("Create")]
        [Authorize]
        //GET
        public IActionResult Create(string UserId)
        {
            var SkillLevels = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = SD.Easy, Value=SD.Easy},
                new SelectListItem { Selected = false, Text = SD.Moderate, Value=SD.Moderate},
                new SelectListItem { Selected = false, Text = SD.Difficult, Value=SD.Difficult}
            }, "Value", "Text");
            ViewBag.SkillLevel = SkillLevels;

            var ActivityTypes = _activityTypeService.ActivityType.GetAll();
            var ActivityTypesList = new SelectList(ActivityTypes, "Id", "Type");
            ViewBag.UserId = UserId;
            ViewBag.ActivityTypesList = ActivityTypesList;
            return View();
        }

        //POST
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Activity obj, IFormFile? file)
        {
            if (obj.DateTo < obj.DateFrom)
            {
                ModelState.AddModelError(nameof(Activity.DateTo), "Datum 'Do' ne sme biti pre datuma 'Od'.");

				
            }
            else if(ModelState.IsValid)
                {
					string wwwRootPath = _hostEnvironment.WebRootPath;
                    string defaultImagePath = @"\default-images\no-image.jpg";
					if (file != null)
					{
						string fileName = Guid.NewGuid().ToString();
						var uploads = Path.Combine(wwwRootPath, @"activity-images");
						var extension = Path.GetExtension(file.FileName);
						using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
						{
							file.CopyTo(fileStream);
						}
						obj.ImageURL = @"\activity-images\" + fileName + extension;
					}
                    else
                    {
                        obj.ImageURL = defaultImagePath;
                    }


					_service.Activity.Add(obj);
					_service.Activity.Save();
					return RedirectToAction("MyActivities", new { UserId = obj.UserId });

		    }
               
            
            return View(obj);
        }

        [HttpGet("Edit")]
        [Authorize]
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _service.Activity.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
                return NotFound();
            return View(obj);
        }

        //POST
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Activity obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var existingActivity = _service.Activity.GetFirstOrDefault(u => u.Id == obj.Id);

                if (existingActivity != null)
                {
                    existingActivity.Name = obj.Name;
                    existingActivity.Description = obj.Description;
                    existingActivity.Location = obj.Location;
                    existingActivity.Address = obj.Address;
                    existingActivity.Price = obj.Capacity;
                    existingActivity.SkillLevel = obj.SkillLevel;
                    existingActivity.DateFrom = obj.DateFrom;
                    existingActivity.DateTo = obj.DateTo;
                    existingActivity.CurrentNumberPeople = obj.CurrentNumberPeople;
                    existingActivity.ActivityTypeId = obj.ActivityTypeId;
                    existingActivity.ActivityType = obj.ActivityType;
                    existingActivity.UserId = obj.UserId;
                    existingActivity.User = obj.User;

                    if (file != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"activity-images");
                        var extension = Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        existingActivity.ImageURL = @"\activity-images\" + fileName + extension;
                    }
                    if (!string.IsNullOrEmpty(obj.ImageURL))
                    {
                        existingActivity.ImageURL = obj.ImageURL;
                    }

                    _service.Activity.Update(existingActivity);
                    _service.Activity.Save();
                    return RedirectToAction("MyActivities", new { UserId = obj.UserId });
                }
                else
                {
                    return NotFound();
                }
            }

            return View(obj);
        }


        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int? id)
		{
			var obj = _service.Activity.GetFirstOrDefault(u => u.Id == id);
			if (obj == null)
			{
				return Json(new { success = false, message = "Error while deleting!" });
			}
            if (obj.ImageURL != null)
            {
                _service.DeletePicture(obj.ImageURL);
            }
            _service.RemoveActivity(id.Value);
            _service.Activity.Remove(obj);
			_service.Activity.Save();
            return Json(new { success = true, redirectUrl = Url.Action("MyActivities", "Activity", new { UserId = obj.UserId }) });

           
        }

        //GET
        [HttpGet]
        public IActionResult ActivityPage(int? ActivityId)
        {

            if (ActivityId == null || ActivityId == 0)
            {
                return NotFound();
            }
            ShoppingCart cart = new ShoppingCart();

            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (_cartService.ActivityInCart(ActivityId))
                {
                    cart = _cartService.GetCartbyActivityId(ActivityId);
                }
                else if (_cartService.IsUserInCart(claim.Value))
                {
                    cart = _cartService.GetActivityCartByUser(claim.Value);
                    if(cart==null)
                    {
                        cart = new ShoppingCart();
                        cart.NumberOfPersons = 1;
                    }
                    
                }
                else
                {
                    cart.NumberOfPersons = 1;
                }
                cart.ApplicationUserId = claim.Value;
            }
            else
               
            {
                cart.ActivityId = ActivityId;
                cart.NumberOfPersons = 1;
            }


            cart.ActivityId = ActivityId;
			cart.Activity = _service.GetActivityById(ActivityId);
            cart.Activity.User = _userService.GetUserById(cart.Activity.UserId);
            cart.Activity.ActivityType = _activityTypeService.GetActivityTypeById(cart.Activity.ActivityTypeId);

            if (cart.Activity == null)
				return NotFound();
			return View(cart);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult ActivityPage(ShoppingCart cart)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			cart.ApplicationUserId = claim.Value;

			if (cart.ActivityId == null)
				return NotFound();
			cart.Activity = null;
			if (cart.Id!=null)
            {
                _cartService.ShoppingCart.Update(cart);
            }
            else
            {
				_cartService.ShoppingCart.Add(cart);
			}
			_cartService.ShoppingCart.Save();

            var activity = _service.GetActivityById(cart.ActivityId);
            if (activity != null && cart.NumberOfPersons.HasValue)
            {
                activity.CurrentNumberPeople += cart.NumberOfPersons.Value;
                _service.Activity.Update(activity);
                _service.Activity.Save();
            }
            return RedirectToAction("Index");
		}


        public ActionResult Filter(int? activityTypeId, string sortOption, string location, int page)
        {
            ActivitiesViewModel viewModel = new ActivitiesViewModel();

            IEnumerable<Activity> filteredActivities;

            if (activityTypeId == null || activityTypeId == 0)
            {
                filteredActivities = _service.Activity.GetAll();
            }
            else
            {
                filteredActivities = _service.GetActivityByActivityType(activityTypeId);
                viewModel.SelectedActivityTypeId = activityTypeId.Value;
            }

            if (!string.IsNullOrEmpty(location))
            {
                filteredActivities = filteredActivities.Where(a => a.Location.Contains(location));
            }

            filteredActivities = SortActivities(filteredActivities, sortOption);

            viewModel.Activities = filteredActivities.Skip((page-1)*5).Take(5);

            IEnumerable<ActivityType> types = _activityTypeService.ActivityType.GetAll();
            viewModel.ActivityTypes = types;

            return PartialView("_ActivityList", viewModel);
        }



        private IEnumerable<Activity> SortActivities(IEnumerable<Activity> activities, string sortOption)
        {
            switch (sortOption)
            {
                case SD.PriceLow:
                    return activities.OrderBy(a => a.Price);
                case SD.PriceHigh:
                    return activities.OrderByDescending(a => a.Price);
                case SD.Old:
                    return activities.OrderBy(a => a.DateFrom);
                case SD.New:
                    return activities.OrderByDescending(a => a.Id);
                case SD.Location:
                    return activities.OrderBy(a => a.Location);
                case SD.Name:
                    return activities.OrderBy(a => a.Name);
                default:
                    return activities.OrderByDescending(a => a.Id);
            }
        }
    }
}

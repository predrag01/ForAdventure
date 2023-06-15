using ForAdventure.Models;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text.Json;
using ForAdventureWeb.Areas.Customer.ViewModels;
using ForAdventure.Service.Utility;
using Microsoft.AspNetCore.Authorization;

namespace ForAdventureWeb.Areas.Customer.Controllers
{
    [Area("Customer")]


    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _service;
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IShoppingCartService _cartService;
        private readonly IApplicationUserService _userService;
        public EquipmentController(IEquipmentService service, IEquipmentTypeService equipmentTypeService, IWebHostEnvironment hostEnvironment, IShoppingCartService cartService,IApplicationUserService applicationUserService)
        {
            _service = service;
            _equipmentTypeService = equipmentTypeService;
            _hostEnvironment = hostEnvironment;
            _cartService = cartService;
            _userService = applicationUserService;
        }
        public IActionResult Index()
        {
            IEnumerable<Equipment> equipment = _service.Equipment.GetAll().Reverse();
            IEnumerable<EquipmentType> types = _equipmentTypeService.EquipmentType.GetAll();
            EquipmentsViewModel viewModel = new EquipmentsViewModel()
            {
                Equipments = equipment.Take(5),
                EquipmentTypes = types
            };
            return View(viewModel);
        }
        [Authorize]
        public IActionResult MyEquipments(string UserId)
        {
            IEnumerable<Equipment> objEquipmentList = _service.GetEquipmentsByUser(UserId);
            objEquipmentList = objEquipmentList.Reverse();
            return View(objEquipmentList);
        }
        //GET
        [Authorize]
        public IActionResult Create(string? OwnerId)
        {
            var EquipmentTypes = _equipmentTypeService.EquipmentType.GetAll();
            var EquipmentTypesList = new SelectList(EquipmentTypes, "Id", "Naziv");
            ViewBag.OwnerId = OwnerId;
            ViewBag.EquipmentTypesList = EquipmentTypesList;
            return View();
        }

        //POST 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Equipment equipment, List<IFormFile>? files)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                List<string> imageUrls = new();
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"images\equipments");
                        var extension = Path.GetExtension(file.FileName);
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }
                        imageUrls.Add(@"\images\equipments\" + fileName + extension);
                    }
                }
                equipment.ImageURLsJson = JsonSerializer.Serialize(imageUrls);

                _service.Equipment.Add(equipment);
                _service.Equipment.Save();
                return RedirectToAction("MyEquipments", new { UserId = equipment.OwnerId });
            }
            return View(equipment);
        }

        //GET
        [Authorize]
        public IActionResult Edit(int id)
        {
            var EquipmentFromDb = _service.Equipment.GetFirstOrDefault(u => u.ID == id);
            var EquipmentTypes = _equipmentTypeService.EquipmentType.GetAll();
            var EquipmentTypesList = new SelectList(EquipmentTypes, "Id", "Naziv");
            ViewBag.EquipmentTypesList = EquipmentTypesList;

            if (EquipmentFromDb == null) { return NotFound(); }


            return View(EquipmentFromDb);
        }

        //POST 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _service.Equipment.Update(equipment);
                _service.Equipment.Save();
            }
            return RedirectToAction("MyEquipments", new { UserId = equipment.OwnerId });
        }


        //GET
        [Authorize]
        public IActionResult Delete(int? id)
        {
            var obj = _service.Equipment.GetFirstOrDefault(u => u.ID == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            if (obj.ImageURLs != null)
            {
                _service.DeletePicture(obj.ImageURLs);
            }
            _service.RemoveEquipment(id.Value);
            _service.Equipment.Remove(obj);
            _service.Equipment.Save();
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Equipment") });
        }

        public IActionResult EquipmentPage(int? EquipmentId)
        {
            ShoppingCart cart = new ShoppingCart();
            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (EquipmentId == null || EquipmentId == 0)
                {
                    return NotFound();
                }
                
                if (_cartService.EquipmentInCart(EquipmentId))
                {
                    cart = _cartService.GetCartbyEquipmentId(EquipmentId);
                }
                else if (_cartService.IsUserInCart(claim.Value))
                {
                    cart = _cartService.GetEquipmentCartByUser(claim.Value);
                    if (cart == null)
                    {
                        cart = new ShoppingCart();
                    }
                }
                else
                {
                    cart = new ShoppingCart();
                }
                cart.ApplicationUserId = claim.Value;
                
            }
            if(cart==null)
            {
                cart = new ShoppingCart();
            }
            cart.EquipmentId = EquipmentId;
            cart.Equipment = _service.GetEquipmentById(EquipmentId);
            cart.Equipment.Owner = _userService.GetUserById(cart.Equipment.OwnerId);
            cart.Equipment.EqType = _equipmentTypeService.GetEquipmentTypeById(cart.Equipment.EqTypeId);

            if (cart.Equipment == null)
                return NotFound();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult EquipmentPage(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.ApplicationUserId = claim.Value;
            if (cart.EquipmentId == null)
                return NotFound();
            cart.Equipment = null;

            if (cart.Id != 0) 
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
        public ActionResult Filter(int? equipmentTypeId, string sortOption, int page)
        {
            EquipmentsViewModel viewModel = new EquipmentsViewModel();

            if (equipmentTypeId == null || equipmentTypeId == 0)
            {
                IEnumerable<Equipment> filteredEquipments = _service.Equipment.GetAll();

                filteredEquipments = SortEquipments(filteredEquipments, sortOption);

                viewModel.Equipments = filteredEquipments.Skip((page-1)*5).Take(5);

                return PartialView("_FilteredEquipmentList", viewModel);
            }
            else
            {
                IEnumerable<Equipment> filteredEquipments = _service.GetEquipmentByEqType(equipmentTypeId);

                filteredEquipments = SortEquipments(filteredEquipments, sortOption);

                viewModel.Equipments = filteredEquipments.Skip((page - 1) * 5).Take(5);

                return PartialView("_FilteredEquipmentList", viewModel);
            }
        }

        private IEnumerable<Equipment> SortEquipments(IEnumerable<Equipment> equipments, string sortOption)
        {
            switch (sortOption)
            {
                case SD.PriceLow:
                    return equipments.OrderBy(e => e.Cena);
                case SD.PriceHigh:
                    return equipments.OrderByDescending(e => e.Cena);
                case SD.Old:
                    return equipments.OrderBy(e => e.ID);
                case SD.Newest:
                    return equipments.OrderByDescending(e => e.ID);
                case SD.Location:
                    return equipments.OrderBy(e => e.Lokacija);
                case SD.Name:
                    return equipments.OrderBy(e => e.Naziv);
                default:
                    return equipments.OrderByDescending(e => e.ID);
            }
        }
    }
}

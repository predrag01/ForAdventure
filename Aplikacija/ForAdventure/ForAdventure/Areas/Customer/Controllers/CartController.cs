using ForAdventure.Models;
using ForAdventure.Models.ViewModels;
using ForAdventure.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace ForAdventureWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
	[Authorize]
	public class CartController : Controller
    {
        private readonly IShoppingCartService _service;
		private readonly IActivityService _serviceActivity;
		private readonly IHotelService _serviceHotel;
		private readonly IEquipmentService _equipment;
		private readonly IApplicationUserService _serviceUser;
        private readonly IRoomService _serviceRoom;
		private readonly IActivityReservationService _ActivityReservation;
		private readonly IEquipmentReservationService _EquipmentReservation;
		private readonly IRoomReservationService _RoomReservation;
        private readonly IEmailSender emailSernder;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IShoppingCartService service, IActivityService serviceActivity, IHotelService serviceHotel, IEquipmentService eq,
            IApplicationUserService servi, IRoomService serviceRoom,
			IActivityReservationService ActivityReservation, IEquipmentReservationService EquipmentReservation, IRoomReservationService RoomReservation, IEmailSender email)
        {

			_service = service;
			_serviceActivity = serviceActivity;
			_serviceHotel = serviceHotel;
			_equipment = eq;
            _serviceUser = servi;
            _serviceRoom = serviceRoom;
			_ActivityReservation = ActivityReservation;
			_EquipmentReservation = EquipmentReservation;
			_RoomReservation = RoomReservation;
            emailSernder = email;
        }

        public IActionResult Index()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                CartList = _service.GetOrdersByUserId(claim.Value),
                CartTotal = 0
            };
            foreach(var obj in ShoppingCartVM.CartList)
            {
                if(obj.ActivityId!=null)
                {
                    obj.Activity = _serviceActivity.GetActivityById(obj.ActivityId);
                    obj.Activity.User = _serviceUser.GetUserById(obj.Activity.UserId);
                    ShoppingCartVM.CartTotal += obj.Activity.Price*obj.NumberOfPersons.Value;
                }
                if(obj.EquipmentId!=null)
                {
                    obj.Equipment = _equipment.GetEquipmentById(obj.EquipmentId);
					obj.DaysEquipment = (obj.DateToEquipment.GetValueOrDefault() - obj.DateFromEquipment.GetValueOrDefault()).Days;
                    ShoppingCartVM.CartTotal += obj.Equipment.Cena * obj.DaysEquipment;
                    obj.Equipment.Owner = _serviceUser.GetUserById(claim.Value);
				}
                if(obj.RoomId!=null)
                {
                    obj.Room = _serviceRoom.GetRoomById(obj.RoomId);
                    obj.Room.Hotel = _serviceHotel.GetHotelById(obj.Room.HotelId);
                    obj.DaysHotel=(obj.DateToRoom.GetValueOrDefault()-obj.DateFromRoom.GetValueOrDefault()).Days;
                    obj.Room.Hotel.Owner= _serviceUser.GetUserById(claim.Value);
                    ShoppingCartVM.CartTotal += obj.Room.PricePerNight * obj.DaysHotel;
                }
            }
			return View(ShoppingCartVM);
        }

        [HttpDelete]
		public IActionResult Delete( int? cartId, int? activityId)
        {
            var cart = _service.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if (cart == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            if (cart.ActivityId == activityId)
            {
                if(cart.RoomId==null && cart.EquipmentId==null)
                {
                    _service.ShoppingCart.Remove(cart);
                }
                else
                {
					cart.Activity = null;
					cart.ActivityId = null;
					cart.NumberOfPersons = null;
					_service.ShoppingCart.Update(cart);
				}
            }
            else if (cart.EquipmentId == activityId)
			{
				if (cart.RoomId == null && cart.ActivityId== null)
				{
					_service.ShoppingCart.Remove(cart);
				}
				else
				{
					cart.Equipment = null;
					cart.EquipmentId = null;
					cart.DateFromEquipment = null;
                    cart.DateToEquipment = null;
					_service.ShoppingCart.Update(cart);
				}
			}
            else if (cart.RoomId == activityId)
			{
				if (cart.EquipmentId == null && cart.ActivityId == null)
				{
					_service.ShoppingCart.Remove(cart);
				}
				else
				{
					cart.Room = null;
					cart.RoomId = null;
					cart.DateFromRoom = null;
					cart.DateToRoom = null;
					_service.ShoppingCart.Update(cart);
				}
			}
			_service.ShoppingCart.Save();
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Pay(ShoppingCartVM shoppingCart)
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			
            ShoppingCartVM = new ShoppingCartVM()
			{
				CartList = _service.GetOrdersByUserId(claim.Value),
				CartTotal = shoppingCart.CartTotal
			};
			foreach (var item in ShoppingCartVM.CartList)
			{
				if(item.ActivityId!=null)
				{
					item.Activity = _serviceActivity.GetActivityById(item.ActivityId);
					item.PriceActivity = (item.NumberOfPersons * item.Activity.Price).Value;
				}
				if(item.EquipmentId!=null)
				{
					item.Equipment=_equipment.GetEquipmentById(item.EquipmentId);
					item.DaysEquipment = (item.DateToEquipment.GetValueOrDefault() - item.DateFromEquipment.GetValueOrDefault()).Days;
					item.PriceEquipment = (item.DaysEquipment * item.Equipment.Cena);
				}
				if (item.RoomId != null)
				{
                    item.Room = _serviceRoom.GetRoomById(item.RoomId);
                    item.Room.Hotel = _serviceHotel.GetHotelById(item.Room.HotelId);
					item.DaysHotel = (item.DateToRoom.GetValueOrDefault() - item.DateFromRoom.GetValueOrDefault()).Days;
					
					item.PriceHotel = item.DaysHotel * item.Room.PricePerNight;
				}
			}

			//stripe payment
			var domain = "https://localhost:44389/";
			//var domain = "https://localhost:7108/";
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes=new List<string>
				{
					"card",
				},
				LineItems = new List<SessionLineItemOptions>(),
				
				Mode = "payment",
				SuccessUrl = domain+$"Customer/Cart/PaymentConfirmation",
				CancelUrl = domain+"Customer/Cart/Index",
			};

			foreach(var item in ShoppingCartVM.CartList)
			{

				if (item.ActivityId != null)
				{
					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(item.Activity.Price * 100),
							Currency = "eur",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Activity.Name,
							},
						},
						Quantity = item.NumberOfPersons,
					};
					options.LineItems.Add(sessionLineItem);
				}
				if (item.EquipmentId != null)
				{
					var sessionLineItem1 = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(item.Equipment.Cena * 100),
							Currency = "eur",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Equipment.Naziv,
							},
						},
						Quantity = item.DaysEquipment,
					};
					options.LineItems.Add(sessionLineItem1);
				}
				if (item.RoomId != null)
				{
					var sessionLineItem2 = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(item.Room.PricePerNight * 100),
							Currency = "eur",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Room.Hotel.Name,
							},
						},
						Quantity = item.DaysHotel,
					};
					options.LineItems.Add(sessionLineItem2);
				}
			}

			var service = new SessionService();
			Session session = service.Create(options);

			foreach (var item in ShoppingCartVM.CartList)
			{
				item.SessionId = session.Id;
				item.PaymentIntentId = session.PaymentIntentId;
				_service.ShoppingCart.Save();
			}

			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
        }
		public IActionResult PaymentConfirmation()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM = new ShoppingCartVM()
			{
				CartList = _service.GetOrdersByUserId(claim.Value),
			};
			var service = new SessionService();
			Session session = service.Get(ShoppingCartVM.CartList.FirstOrDefault().SessionId);

			IList<ConfirmationVM> list=new List<ConfirmationVM>();
            if (session.PaymentStatus.ToLower() == "paid")
			{
				foreach (var item in ShoppingCartVM.CartList)
				{
					if (item.ActivityId != null)
					{
						item.Activity = _serviceActivity.GetActivityById(item.ActivityId);
						item.PriceActivity = (item.NumberOfPersons * item.Activity.Price).Value;
						ActivityReservation reservation = new ActivityReservation()
						{
							ActivityId = item.ActivityId.Value,
							ApplicationUserId = item.ApplicationUserId,
							TotalPrice = item.PriceActivity,
							NumberOfPeople = item.NumberOfPersons.Value
						};
						_ActivityReservation.ActivityReservation.Add(reservation);
						_ActivityReservation.ActivityReservation.Save();

						var it = new ConfirmationVM()
						{
							Name = item.Activity.Name,
							Count = item.NumberOfPersons.Value,
							DateStart = item.Activity.DateFrom,
							DateEnd = item.Activity.DateTo,
							TotalPrice=item.Activity.Price
						};
						list.Add(it);
					}
					if (item.EquipmentId != null)
					{
						item.Equipment = _equipment.GetEquipmentById(item.EquipmentId);
						item.DaysEquipment = (item.DateToEquipment.GetValueOrDefault() - item.DateFromEquipment.GetValueOrDefault()).Days;
						item.PriceEquipment = (item.DaysEquipment * item.Equipment.Cena);
						EquipmentReservation reservation = new EquipmentReservation()
						{
							ReservedFrom = item.DateFromEquipment,
							ReservedUntil = item.DateToEquipment,
							EquipmentId = item.EquipmentId.Value,
							ApplicationUserId = item.ApplicationUserId,
							TotalPrice = item.PriceEquipment,
						};
						_EquipmentReservation.EquipmentReservation.Add(reservation);
						_EquipmentReservation.EquipmentReservation.Save();

                        var it = new ConfirmationVM()
                        {
                            Name = item.Equipment.Naziv,
                            Count = (item.DateToEquipment.GetValueOrDefault() - item.DateFromEquipment.GetValueOrDefault()).Days,
                            DateStart = item.DateFromEquipment.Value,
                            DateEnd = item.DateToEquipment.Value,
							TotalPrice=item.Equipment.Cena
                        };
                        list.Add(it);
                    }
					if (item.RoomId != null)
					{
                        item.Room = _serviceRoom.GetRoomById(item.RoomId);
                        item.Room.Hotel = _serviceHotel.GetHotelById(item.Room.HotelId);
						item.DaysHotel = (item.DateToRoom.GetValueOrDefault() - item.DateFromRoom.GetValueOrDefault()).Days;
						
						item.PriceHotel = item.DaysHotel * item.Room.PricePerNight;
						RoomReservation reservation = new RoomReservation()
						{
							ReservedFrom = item.DateFromRoom,
							ReservedUntil = item.DateToRoom,
							RoomId = item.RoomId.Value,
							UserId = item.ApplicationUserId,
							TotalPrice = item.PriceHotel,
						};
						_RoomReservation.RoomReservation.Add(reservation);
						_RoomReservation.RoomReservation.Save();

                        var it = new ConfirmationVM()
                        {
                            Name = item.Room.Hotel.Name,
                            Count = (item.DateToRoom.GetValueOrDefault() - item.DateFromRoom.GetValueOrDefault()).Days,
                            DateStart = item.DateFromRoom.Value,
                            DateEnd = item.DateToRoom.Value,
							TotalPrice=item.Room.PricePerNight
                        };
                        list.Add(it);
                    }
				}
			}
			emailSernder.SendEmailAsync(_serviceUser.GetEmailByUserId(claim.Value), "New reservation - ForAdventure", _service.FormatMessage(list));
			_service.ShoppingCart.RemoveRange(ShoppingCartVM.CartList.ToList());
			_service.ShoppingCart.Save();
			return View(list);
		}

		public IActionResult CheckAvailability()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			IEnumerable<ShoppingCart> cart = _service.GetOrdersByUserId(claim.Value);
			foreach(ShoppingCart cartItem in cart)
			{
				if(cartItem.ActivityId!=null)
				{
					var act = _serviceActivity.GetActivityById(cartItem.ActivityId);
					if(!_ActivityReservation.Check(cartItem.ActivityId.Value, cartItem.NumberOfPersons.Value, act.Capacity))
					{
						return Json(new { success = false, message="Not enough places available!" });
					}
				}
				if(cartItem.EquipmentId!=null)
				{
					if(!_EquipmentReservation.Check(cartItem.EquipmentId.Value, cartItem.DateFromEquipment.Value, cartItem.DateToEquipment.Value))
					{
						return Json(new { success = false, message = "This date for equipment is already reserved! Choose another date." });
					}
				}
				if(cartItem.RoomId!=null)
				{
					if(!_RoomReservation.Check(cartItem.RoomId.Value, cartItem.DateFromRoom.Value, cartItem.DateToRoom.Value))
					{
						return Json(new { success = false, message = "This date for room is already reserved! Choose another date." });
					}
				}
			}
			return Json(new { success = true });
		}
	}
}

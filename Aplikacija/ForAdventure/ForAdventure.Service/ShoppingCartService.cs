using ForAdventure.DataAccess.Data;
using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Models.ViewModels;
using ForAdventure.Service.IService;
using System.Globalization;

namespace ForAdventure.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private ApplicationDbContext _db;

        public IShoppingCartRepository ShoppingCart { get; private set; }
        public ShoppingCartService(ApplicationDbContext db, IShoppingCartRepository shoppingCart)
        {
            _db = db;
            ShoppingCart = shoppingCart;
        }

		public IEnumerable<ShoppingCart> GetOrdersByUserId(string id)
		{
            IEnumerable<ShoppingCart> list = ShoppingCart.GetAll().Where(x=>x.ApplicationUserId == id);
            return list;
		}

		public bool InCart(int? id)
		{
            if(ShoppingCart.GetFirstOrDefault(x=>x.ActivityId== id)!=null)
            {
                return true;
            }
            if(ShoppingCart.GetFirstOrDefault(x => x.EquipmentId == id) != null)
			{
				return true;
			}
			if (ShoppingCart.GetFirstOrDefault(x => x.Room.HotelId == id) != null)
			{
				return true;
			}
			return false;
		}

		public ShoppingCart GetCartbyId(int? id)
		{
			return ShoppingCart.GetFirstOrDefault(x=>x.Id==id);
		}

		public ShoppingCart GetCartbyActivityId(int? id)
		{
			return ShoppingCart.GetFirstOrDefault(x => x.ActivityId == id);
		}

		public ShoppingCart GetCartbyEquipmentId(int? id)
		{
			return ShoppingCart.GetFirstOrDefault(x => x.EquipmentId == id);
		}

		public ShoppingCart GetCartbyHotelId(int? id)
		{
			return ShoppingCart.GetFirstOrDefault(x => x.RoomId == id);
		}

		public ShoppingCart GetActivityCartByUser(string id)
		{
			return ShoppingCart.GetAll().Where(x=>x.ApplicationUserId==id).Where(x=>x.ActivityId==null).FirstOrDefault();
		}

		public ShoppingCart GetEquipmentCartByUser(string id)
		{
			return ShoppingCart.GetAll().Where(x => x.ApplicationUserId == id).Where(x => x.EquipmentId == null).FirstOrDefault();
		}

		public ShoppingCart GetHotelCartByUser(string id)
		{
			return ShoppingCart.GetAll().Where(x => x.ApplicationUserId == id).Where(x => x.RoomId == null).FirstOrDefault();
		}

		public bool ActivityInCart(int? id)
		{
			if (ShoppingCart.GetFirstOrDefault(x => x.ActivityId == id) != null)
			{
				return true;
			}
			return false;
		}

		public bool EquipmentInCart(int? id)
		{
			if (ShoppingCart.GetFirstOrDefault(x => x.EquipmentId == id) != null)
			{
				return true;
			}
			return false;
		}

		public bool HotelInCart(int? id)
		{
			if (ShoppingCart.GetFirstOrDefault(x => x.RoomId == id) != null)
			{
				return true;
			}
			return false;
		}
		public bool IsUserInCart(string id)
		{
			var user = ShoppingCart.GetFirstOrDefault(x => x.ApplicationUserId == id);
			if (user != null)
			{
				return true;
			}
			return false;
		}

		public void UpdateSessionPaymentIntent(string userId, string session, string paymentIntent)
		{
			IEnumerable<ShoppingCart> carts=GetOrdersByUserId(userId);
			foreach(var  cart in carts)
			{
				cart.SessionId = session;
				cart.PaymentIntentId= paymentIntent;
			}
		}

        public string FormatMessage(IList<ConfirmationVM> list)
        {
			string message="<div>" +
				"<hr />";
			foreach (var item in list)
			{
				message += "<div>"
					+ "<h4>"+@item.Name.ToString() +"</h4>" +
					"<p class=" + "mb-0" + "><i class=" + "bi bi-calendar3" + "></i>&nbsp; Start: "+@item.DateStart.ToString() +"</p>" +
					"<p class=" + "mb-1" + "><i class=" + "bi bi-calendar3" + "></i>&nbsp; End: "+@item.DateEnd.ToString()+"</p>" +
					"<p>Price: "+@item.Count.ToString()+" x " + @item.TotalPrice.ToString("C", new CultureInfo("en-MNE")) + "</p>" +
					"</div>" +
					"<hr />";
			}
			message += "</div>";
			return message;
        }
    }
}

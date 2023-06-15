using ForAdventure.DataAccess.Repository.IRepository;
using ForAdventure.Models;
using ForAdventure.Models.ViewModels;

namespace ForAdventure.Service.IService
{
    public interface IShoppingCartService
    {
        public IShoppingCartRepository ShoppingCart { get; }

        IEnumerable<ShoppingCart> GetOrdersByUserId(string id);
        bool InCart(int? id);
		bool ActivityInCart(int? id);
		bool EquipmentInCart(int? id);
		bool HotelInCart(int? id);
		ShoppingCart GetCartbyId(int? id);
		ShoppingCart GetCartbyActivityId(int? id);
		ShoppingCart GetCartbyEquipmentId(int? id);
		ShoppingCart GetCartbyHotelId(int? id);
        ShoppingCart GetActivityCartByUser(string id);
		ShoppingCart GetEquipmentCartByUser(string id);
		ShoppingCart GetHotelCartByUser(string id);
		bool IsUserInCart(string id);
		string FormatMessage(IList<ConfirmationVM> list);

    }
}

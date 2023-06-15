namespace ForAdventure.Models.ViewModels
{
    public class ShoppingCartVM
	{
		public IEnumerable<ShoppingCart> CartList { get; set; }
        public double CartTotal { get; set; }
    }
}

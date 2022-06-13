using CoreCodeFirst.Models;

namespace CoreCodeFirst.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart>ListCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
        //public ShoppingCart ShoppingCart { get; set; }
    }
}

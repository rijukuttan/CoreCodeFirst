using CoreCodeFirst.Models;

namespace CoreCodeFirst.ViewModels
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
       // public OrderDetails OrderDetails { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}

using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using System.Linq.Expressions;

namespace CoreCodeFirst.Repository.IRepository
{
  
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    {
        IEnumerable<ShoppingCart> GetCartByUser(Expression<Func<ShoppingCart, bool>>? filter = null, string? includeProperties = null);
        int IncrementCount(ShoppingCart shoppingCart, int count);
       int DecrementCount(ShoppingCart shoppingCart, int count);
    }
}

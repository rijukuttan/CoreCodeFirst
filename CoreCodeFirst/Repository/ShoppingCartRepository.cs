using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoreCodeFirst.Repository
{
   
    public class ShoppingCartRepository:Repository<ShoppingCart>,IShoppingCartRepository
    {
        private readonly DatabaseContext _db;
        internal DbSet<ShoppingCart> dbset;
        public ShoppingCartRepository(DatabaseContext db) : base(db)
        {
            _db = db;
            this.dbset = _db.Set<ShoppingCart>();
        }
        public void Update(ShoppingCart shoppingCart)
        {
            _db.Update(shoppingCart);
        }
     
        public IEnumerable<ShoppingCart> GetCartByUser(Expression<Func<ShoppingCart, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<ShoppingCart> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);

                }
            }
            return query.ToList();
        }
        public int DecrementCount(ShoppingCart shoppingCart, int count){
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }
        public int IncrementCount(ShoppingCart shoppingCart, int count){
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }
    }

}

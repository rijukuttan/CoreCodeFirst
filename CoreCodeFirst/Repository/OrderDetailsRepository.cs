using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;

namespace CoreCodeFirst.Repository
{
 /*   public class CategoryRepository
    {
    }*/
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly DatabaseContext _db;

        public OrderDetailsRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }
        /*     public void Save()
             {
                 _db.SaveChanges();
             }*/

        public void Update(OrderDetails OrderDetails)
        {
            _db.Update(OrderDetails);
        }


    }
}

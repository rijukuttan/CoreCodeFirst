//using CoreCodeFirst.Data;
using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;
//using CoreCodeFirst.Repository.IRepository;
using System.Linq.Expressions;

namespace CoreCodeFirst.Repository
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private readonly DatabaseContext _db;

        public ProductRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }
        /*     public void Save()
             {
                 _db.SaveChanges();
             }*/

        public void Update(Product product)
        {
           _db.Update(product);
        }

       
    }
}

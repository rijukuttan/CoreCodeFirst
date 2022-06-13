using CoreCodeFirst.Models;

namespace CoreCodeFirst.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product product);
       // void Save();
       
    }
}

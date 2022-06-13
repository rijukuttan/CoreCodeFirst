using CoreCodeFirst.Models;

namespace CoreCodeFirst.Repository.IRepository
{
  
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails OrderDetails);
        // void Save();

    }
}

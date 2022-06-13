using CoreCodeFirst.Models;

namespace CoreCodeFirst.Repository.IRepository
{
  
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
        // void Save();

    }
}

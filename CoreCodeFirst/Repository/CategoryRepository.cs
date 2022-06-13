using CoreCodeFirst.Data;
using CoreCodeFirst.Models;
using CoreCodeFirst.Repository.IRepository;

namespace CoreCodeFirst.Repository
{
 /*   public class CategoryRepository
    {
    }*/
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly DatabaseContext _db;

        public CategoryRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }
        /*     public void Save()
             {
                 _db.SaveChanges();
             }*/

        public void Update(Category category)
        {
           
                _db.Update(category);
          
        }


    }
}

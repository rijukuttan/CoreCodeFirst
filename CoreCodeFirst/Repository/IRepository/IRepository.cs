using System.Linq.Expressions;

namespace CoreCodeFirst.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null,string? includeProperties=null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter,string? includeProperties = null);
        void Add(T item);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);
    }
}

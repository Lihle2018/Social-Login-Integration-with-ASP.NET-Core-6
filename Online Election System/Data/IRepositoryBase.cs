using System.Linq.Expressions;

namespace Online_Election_System.Data
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> GetById(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}

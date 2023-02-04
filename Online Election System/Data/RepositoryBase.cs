using System.Linq.Expressions;

namespace Online_Election_System.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AppDbContext _appDbContext;
        public RepositoryBase(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Add(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _appDbContext.Set<T>().Where(expression);
        }

        public IEnumerable<T> GetAll()
        {
            return _appDbContext.Set<T>().ToList();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);

        }

        public void Remove(T entity)
        {
            _appDbContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _appDbContext.Set<T>().RemoveRange(entities);
        }
    }
}

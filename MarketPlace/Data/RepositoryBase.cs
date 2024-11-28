using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MarketPlace.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public RepositoryBase(DbContext context) 
        {
            _context = context;
            _dbSet = _context.Set<T>(); ;
        }


        public async Task< IEnumerable<T>> FindAll()
        {
            return await _dbSet.ToListAsync();
        }


        public T GetById(int id)
        {
            return  _dbSet.Find(id);
        }

        async Task IRepositoryBase<T>.Create(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        async Task IRepositoryBase<T>.Delete(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        async Task IRepositoryBase<T>.Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

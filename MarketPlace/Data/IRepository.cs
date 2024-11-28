using System.Linq.Expressions;

namespace MarketPlace.Data
{
    public interface IRepositoryBase<T> where T : class
    {
        T GetById(int id);
        Task<IEnumerable<T>> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}

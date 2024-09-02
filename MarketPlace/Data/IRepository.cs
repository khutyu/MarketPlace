using System.Linq.Expressions;

namespace MarketPlace.Data
{
    public interface IRepositoryBase<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

using System.Linq.Expressions;

namespace MarketPlace.Shared
{
    public interface IRepositoryBase<T>
    {
        T GetById(int id);
        T GetById(string id);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}

using System.Linq;

namespace WebService.Infrastructure.DataAccess
{
    public interface IRepository<T> where T : Entity, IObjectWithState
    {
        T Get(long id);
        T Get(string includePath, long id);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(string includePath);
        void AddOrUpdate(T entity);
        void Remove(long id);
        void SubmitChanges();
    }
}
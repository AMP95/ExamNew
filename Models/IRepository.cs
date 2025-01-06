using System.Linq.Expressions;

namespace Models
{
    public interface IRepository
    {
        Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> filter = null,
                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                 string includeProperties = "") where T : BaseEntity;

        Task<IEnumerable<T>> GetRange<T>(int start, int count, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "") where T : BaseEntity;

        Task<T> GetById<T>(Guid id) where T : BaseEntity;

        Task<bool> Update<T>(T entity) where T : BaseEntity;

        Task<bool> Remove<T>(Guid id) where T : BaseEntity;
    }
}

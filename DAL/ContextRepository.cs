using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace DAL
{
    public class ContextRepository : IRepository
    {
        private Context _context;
        private ILogger<ContextRepository> _logger;

        public ContextRepository(Context context, ILogger<ContextRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
                                                 IOrderedQueryable<T>> orderBy = null,
                                                 string includeProperties = "") where T : BaseEntity
        {
            if (_context != null)
            {
                IQueryable<T> query = _context.Set<T>();
                if (query.Any())
                {
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }

                    foreach (string includeProperty in includeProperties.Split
                        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }

                    if (orderBy != null)
                    {
                        return orderBy(query);
                    }
                    else
                    {
                        return query;
                    }
                }
            }
            return Enumerable.Empty<T>();
        }

        public async Task<IEnumerable<T>> GetRange<T>(int start, int end, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "") where T : BaseEntity
        {
            if (_context != null)
            {
                IQueryable<T> query = _context.Set<T>();
                if (query.Any())
                {
                    foreach (string includeProperty in includeProperties.Split
                        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }

                    if (orderBy != null)
                    {
                        query = orderBy(query);
                    }

                    if (end > start)
                    {
                        return query.Skip(start).Take(end);
                    }
                    else
                    {
                        return query;
                    }
                }
            }
            return Enumerable.Empty<T>();
        }

        public async Task<T> GetById<T>(Guid id) where T : BaseEntity
        {
            if (_context != null)
            {
                return await _context.Set<T>().FindAsync(id);
            }
            return null;
        }

        public async Task<bool> Remove<T>(Guid id) where T : BaseEntity
        {
            if (_context != null)
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    T entity = await _context.Set<T>().FindAsync(id);
                    if (entity != null)
                    {
                        try
                        {
                            _context.Set<T>().Remove(entity);
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                            await transaction.RollbackAsync();
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> Update<T>(T entity) where T : BaseEntity
        {
            if (_context != null)
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Set<T>().Update(entity);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        await transaction.RollbackAsync();
                    }
                }
            }
            return false;
        }

        public async Task<Guid> Add<T>(T entity) where T : BaseEntity
        {
            if (_context != null)
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Set<T>().Update(entity);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return entity.Id;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        await transaction.RollbackAsync();
                    }
                }
            }
            return Guid.Empty;
        }
    }
}

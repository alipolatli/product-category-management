using product_category.core.Domain.EFCore.SeedWork;
using System.Linq.Expressions;

namespace product_category.core.Abstractions.Repositories.EFCore;

public interface IEFGenericRepository<T> : IRepository<T> where T : EFEntity
{
	IQueryable<T> AsQueryable(Expression<Func<T, bool>>? filter = null);

	Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
	Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes);

	Task<PaginatedResult<T>> GetAllPaginatedAsync(int page, int size, CancellationToken cancellationToken = default);
	Task<PaginatedResult<T>> GetAllPaginatedAsync(int page, int size, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
	Task<PaginatedResult<T>> GetAllPaginatedOrderingAsync(int page, int size, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes);


	Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
	Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

	Task<bool> AnyAsync(int id, CancellationToken cancellationToken = default);
	Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

	Task<long> TotalCountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

	Task AddAsync(T entity, CancellationToken cancellationToken = default);
	Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

	void Update(T entity);
	void UpdateRange(IEnumerable<T> entities);

	void Remove(T entity);
	void RemoveRange(IEnumerable<T> entities);
}

public record PaginatedResult<T>(IEnumerable<T> data, long total);

using product_category.core.Domain.ES.SeedWork;
using System.Linq.Expressions;

namespace product_category.core.Abstractions.Repositories.ES;

public interface IESGenericRepository<T> where T : ESEntity
{
	Task<string?> AddAsync(T entity,CancellationToken cancellationToken = default);
	Task<bool> BulkAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
	Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
	Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
	Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
	Task<T?> GetByIdsAsync(params (Expression<Func<T, string>> fieldSelector, string value)[] fieldSelectors);

	Task<bool> IsExistsByIdAsync(Expression<Func<T, string>> fieldSelector, string value, CancellationToken cancellationToken = default);
	Task<bool> IsExistsByIdsAsync(params (Expression<Func<T, string>> fieldSelector, string value)[] fieldSelectors);
}

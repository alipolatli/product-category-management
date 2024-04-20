namespace product_category.core.Domain.EFCore.SeedWork;
public interface IRepository<T> where T : EFEntity
{
	IUnitOfWork UnitOfWork { get; }
}

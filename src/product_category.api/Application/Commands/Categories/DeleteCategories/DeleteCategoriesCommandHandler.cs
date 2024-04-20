using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;

namespace product_category.api.Application.Commands.Categories.DeleteCategories;

public class DeleteCategoryCommandHandler : CommandHandler, IRequestHandler<DeleteCategoriesCommand, InventoryUploadTrackingObject>
{
	private readonly IEFCategoryRepository _eFCategoryRepository;
	private readonly IValidator<DeleteCategoryRequest> _validator;

	public DeleteCategoryCommandHandler(IEFCategoryRepository eFCategoryRepository, IValidator<DeleteCategoryRequest> validator)
	{
		_eFCategoryRepository = eFCategoryRepository;
		_validator = validator;
	}

	public async Task<InventoryUploadTrackingObject> Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
	{
		foreach (var requestCategory in request.Categories)
		{
			ValidationResult result = await _validator.ValidateAsync(requestCategory, cancellationToken);
			if (!result.IsValid)
			{
				AddInventorUploadItem(request: requestCategory, errors: result.FormatValidationResult());
				continue;
			}

			Category category = (await _eFCategoryRepository.GetByIdAsync(requestCategory.CategoryId, cancellationToken))!;
			_eFCategoryRepository.Remove(category);
			await _eFCategoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
			AddInventorUploadItem(request: requestCategory, identifier: category.Id);
		}
		return InventoryUploadTrackingObject;
	}
}

#region Old
//public async Task Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
//{
//	Category? category = await _eFCategoryRepository.GetByIdAsync(request.CategoryId);
//	if (category == null)
//		throw new Exception();

//	if (!await _eFCategoryRepository.ContainsSubCategoriesAsync(category.Id)) throw new Exception();

//	if (await _eFCategoryRepository.ContainsAttributesAsync(category.Id)) throw new Exception();

//	if (await _eFCategoryRepository.ContainsProductsAsync(category.Id)) throw new Exception();

//	_eFCategoryRepository.Delete(category);
//	if (!await _eFCategoryRepository.UnitOfWork.SaveEntitiesAsync()) throw new Exception();
//}
#endregion

#region Old V2
//		 public async Task<InventoryUploadTrackingObject> Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
//{
//	foreach (var requestCategory in request.Categories)
//	{
//		HashSet<string> errors = new HashSet<string>();

//		Category? category = await _eFCategoryRepository.GetByIdAsync(requestCategory.CategoryId);
//		if (category! == null!)
//		{
//			errors.Add("e");
//			base.AddInventorUploadItem(requestCategory, errors, identifiers: null);
//			continue;
//		}

//		await ValidatCategoryAsync(requestCategory.CategoryId, errors, cancellationToken);
//		if (errors.Any())
//		{
//			base.AddInventorUploadItem(requestCategory, errors, identifiers: null);
//			continue;
//		}

//		_eFCategoryRepository.Delete(category);
//		await _eFCategoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
//		base.AddInventorUploadItem(request: requestCategory, errors: errors, identifier: category.Id);
//	}

//	return base.InventoryUploadTrackingObject;
//}

//private async Task ValidatCategoryAsync(int categoryId, HashSet<string> errors, CancellationToken cancellationToken)
//{
//	if (await _inventoryValidationManager.CategoryContainSubCategoriesAsync(categoryId, cancellationToken)) errors.Add("e");

//	if (await _inventoryValidationManager.CategoryContainAttributesAsync(categoryId, cancellationToken)) errors.Add("e");

//	if (await _inventoryValidationManager.CategoryContainProductsAsync(categoryId, cancellationToken)) errors.Add("e");
//}
#endregion

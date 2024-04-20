using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;


namespace product_category.api.Application.Commands.Categories.UpdateCategories
{
	public class UpdateCategoriesCommandHandler : CommandHandler, IRequestHandler<UpdateCategoriesCommand, InventoryUploadTrackingObject>
	{
		private readonly IEFCategoryRepository _eFCategoryRepository;
		private readonly IValidator<UpdateCategoryRequest> _validator;
		public UpdateCategoriesCommandHandler(IEFCategoryRepository eFCategoryRepository, IValidator<UpdateCategoryRequest> validator)
		{
			_eFCategoryRepository = eFCategoryRepository;
			_validator = validator;
		}

		public async Task<InventoryUploadTrackingObject> Handle(UpdateCategoriesCommand request, CancellationToken cancellationToken)
		{
			foreach (var requestCategory in request.Categories)
			{
				ValidationResult result = await _validator.ValidateAsync(requestCategory, cancellationToken);
				if (!result.IsValid)
				{
					base.AddInventorUploadItem(request: requestCategory, errors: result.FormatValidationResult());
					continue;
				}

				Category category = (await _eFCategoryRepository.GetByIdAsync(requestCategory.CategoryId, cancellationToken))!;
				category.ParentId = requestCategory.MoveParentCategoryId.HasValue ? requestCategory.MoveParentCategoryId.Value : category.ParentId;
				category.Name = string.IsNullOrEmpty(category.Name) ? category.Name : requestCategory.Name!;
				await _eFCategoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
				base.AddInventorUploadItem(request: requestCategory, identifier: category.Id);
			}
			return base.InventoryUploadTrackingObject;
		}
	}
}
#region Old
////toDO => send to elasticsearch products index categoryName change.
//public async Task Handle(UpdateCategoriesCommandRequest request, CancellationToken cancellationToken)
//{
//	Category category = await _eFCategoryRepository.GetByIdAsync(request.CategoryId) ?? throw new Exception("Category not found.");

//	//name change
//	category.Name = request.Name ?? category.Name;

//	//move 
//	if (request.MoveParentCategoryId.HasValue)
//		await MoveCategoryAsync(request.MoveParentCategoryId.Value, category);

//	if (!await _eFCategoryRepository.UnitOfWork.SaveEntitiesAsync()) throw new Exception();
//}

//private async Task MoveCategoryAsync(int moveParentCategoryId, Category category)
//{
//	if (!await _eFCategoryRepository.ExistAsync(moveParentCategoryId)) throw new Exception();

//	if (await _eFCategoryRepository.ContainsAttributesAsync(moveParentCategoryId)) throw new Exception();

//	if (await _eFCategoryRepository.ContainsProductsAsync(moveParentCategoryId)) throw new Exception();

//	category.ParentCategoryId = moveParentCategoryId;
//}
#endregion

#region Old V2

//foreach (var requestCategory in request.Categories)
//{
//	HashSet<string> errors = new HashSet<string>();

//	Category? category = await _eFCategoryRepository.GetByIdAsync(requestCategory.CategoryId);
//	if (category! == null!)
//	{
//		errors.Add("e");
//		base.AddInventorUploadItem(requestCategory, errors, identifiers: null);
//		continue;
//	}

//	if (requestCategory.MoveParentCategoryId.HasValue)
//	{
//		await ValidateParentCategoryAsync(requestCategory.MoveParentCategoryId.Value, errors, cancellationToken);
//		if (errors.Any())
//		{
//			base.AddInventorUploadItem(requestCategory, errors, identifiers: null);
//			continue;
//		}
//		category.ParentCategoryId = requestCategory.MoveParentCategoryId.Value;
//	}

//	if (!string.IsNullOrEmpty(requestCategory.Name))
//	{
//		if (!_inventoryValidationManager.ValidateCategoryName(requestCategory.Name))
//		{
//			errors.Add("e");
//			base.AddInventorUploadItem(request: requestCategory, errors: errors, identifiers: null);
//			continue;
//		}
//		category.Name = requestCategory.Name;
//	}


//	await _eFCategoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
//	base.AddInventorUploadItem(request: requestCategory, errors: errors, identifier: category.Id);
//}
//return base.InventoryUploadTrackingObject;
//		}

//		private async Task ValidateParentCategoryAsync(int parentId, HashSet<string> errors, CancellationToken cancellationToken)
//{
//	if (!await _inventoryValidationManager.ExistCategoryAsync(parentId, cancellationToken)) errors.Add("e");

//	if (await _inventoryValidationManager.CategoryContainAttributesAsync(parentId, cancellationToken)) errors.Add("e");

//	if (await _inventoryValidationManager.CategoryContainProductsAsync(parentId, cancellationToken)) errors.Add("e");
//}

#endregion
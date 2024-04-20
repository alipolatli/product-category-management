using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;


namespace product_category.api.Application.Commands.Categories.AddCategories
{
	public class AddCategoriesCommandHandler : CommandHandler, IRequestHandler<AddCategoriesCommand, InventoryUploadTrackingObject>
	{
		private readonly IEFCategoryRepository _eFCategoryRepository;
		private readonly IValidator<AddCategoryRequest> _validator;

		public AddCategoriesCommandHandler(IEFCategoryRepository eFCategoryRepository, IValidator<AddCategoryRequest> validator)
		{
			_eFCategoryRepository = eFCategoryRepository;
			_validator = validator;
		}

		public async Task<InventoryUploadTrackingObject> Handle(AddCategoriesCommand request, CancellationToken cancellationToken)
		{
			foreach (var requestCategory in request.Categories)
			{
				ValidationResult result = await _validator.ValidateAsync(requestCategory);
				if (!result.IsValid)
				{
					var errors = result.FormatValidationResult();
					base.AddInventorUploadItem(request: requestCategory, errors: errors);

					//result.Errors.Select(e => e.FormattedMessagePlaceholderValues);
					//base.AddInventorUploadItem(request: requestCategory, errors: result.Errors.Select(e => e.ErrorMessage).ToList());
					continue;
				}

				Category category = new Category()
				{
					Name = requestCategory.Name,
					ParentId = requestCategory.ParentId.HasValue ? requestCategory.ParentId.Value : null
				};

				await AddSubCategoriesRecursiveAsync(category, requestCategory.SubCategories ?? Enumerable.Empty<AddCategoryRequest>());
				await _eFCategoryRepository.AddAsync(category, cancellationToken);
				await _eFCategoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
				base.AddInventorUploadItem(request: requestCategory, identifiers: new[] { category.Id }.Concat(category.SubCategories.SelectRecursive(c => c.SubCategories).Select(c => c.Id)));
			}
			return base.InventoryUploadTrackingObject;
		}

		private async Task AddSubCategoriesRecursiveAsync(Category parentCategory, IEnumerable<AddCategoryRequest> requestSubCategories)
		{
			foreach (var requestSubCategory in requestSubCategories)
			{
				Category category = new Category
				{
					Name = requestSubCategory.Name,
				};
				parentCategory.SubCategories.Add(category);
				await AddSubCategoriesRecursiveAsync(category, requestSubCategory.SubCategories ?? Enumerable.Empty<AddCategoryRequest>());
			}
		}
	}

}

#region Old

//public async Task<InventoryUploadTrackingObject> Handle(AddCategoriesCommand request, CancellationToken cancellationToken)
//{
//	foreach (var requestCategory in request.Categories)
//	{
//		ValidationResult result = await _validator.ValidateAsync(requestCategory);

//		HashSet<string> errors = new HashSet<string>();
//		await ValidateParentCategoryAsync(requestCategory.ParentId, errors, cancellationToken);
//		if (errors.Any())
//		{
//			base.AddInventorUploadItem(requestCategory, errors, identifiers: null);
//			continue;
//		}

//		if (!_inventoryValidationManager.ValidateCategoryName(requestCategory.Name))
//		{
//			errors.Add("error.");
//			base.AddInventorUploadItem(requestCategory, errors, identifiers: null);
//			continue;
//		}

//		Category category = new Category()
//		{
//			Name = requestCategory.Name,
//			ParentCategoryId = requestCategory.ParentId.HasValue ? requestCategory.ParentId.Value : null
//		};

//		if (requestCategory.SubCategories != null && requestCategory.SubCategories.Any())
//		{
//			await AddSubCategoriesRecursiveAsync(category, requestCategory.SubCategories, errors);
//			if (errors.Any())
//			{
//				base.AddInventorUploadItem(requestCategory, errors, identifiers: null);
//				continue;
//			}
//		}

//		await _eFCategoryRepository.AddAsync(category, cancellationToken);
//		await _eFCategoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

//		base.AddInventorUploadItem(
//				request: requestCategory,
//				errors: errors,
//				identifiers: new[] { category.Id }.Concat(category.SubCategories.SelectRecursive(c => c.SubCategories).Select(c => c.Id)));
//	}

//	return base.InventoryUploadTrackingObject;
//}

//private async Task AddSubCategoriesRecursiveAsync(Category parentCategory, IEnumerable<AddCategoryRequest> requestSubCategories, HashSet<string> errors)
//{
//	foreach (var requestSubCategory in requestSubCategories)
//	{
//		if (!_inventoryValidationManager.ValidateCategoryName(requestSubCategory.Name)) errors.Add("e");
//		if (!_inventoryValidationManager.ValidateSubCategoryParentId(requestSubCategory.ParentId)) errors.Add("e");
//		Category category = new Category
//		{
//			Name = requestSubCategory.Name,
//		};
//		parentCategory.SubCategories.Add(category);
//		await AddSubCategoriesRecursiveAsync(category, requestSubCategory.SubCategories ?? Enumerable.Empty<AddCategoryRequest>(), errors);
//	}
//}

//private async Task ValidateParentCategoryAsync(int? parentId, HashSet<string> errors, CancellationToken cancellationToken)
//{
//	if (parentId.HasValue)
//	{
//		if (!await _inventoryValidationManager.ExistCategoryAsync(parentId.Value, cancellationToken)) errors.Add("e");

//		if (await _inventoryValidationManager.CategoryContainAttributesAsync(parentId.Value, cancellationToken)) errors.Add("e");

//		if (await _inventoryValidationManager.CategoryContainProductsAsync(parentId.Value, cancellationToken)) errors.Add("e");
//	}
//}
#endregion
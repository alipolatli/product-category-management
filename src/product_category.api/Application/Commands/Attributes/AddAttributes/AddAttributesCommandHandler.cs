using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;

namespace product_category.api.Application.Commands.Attributes.AddAttributes;

public class AddAttributesCommandHandler : CommandHandler, IRequestHandler<AddAttributesCommand, InventoryUploadTrackingObject>
{
	private readonly IEFCategoryAttributeRepository _eFCategoryAttributeRepository;
	private readonly IValidator<AddAttributeRequest> _validator;

	public AddAttributesCommandHandler(IEFCategoryAttributeRepository eFCategoryAttributeRepository, IValidator<AddAttributeRequest> validator)
	{
		_eFCategoryAttributeRepository = eFCategoryAttributeRepository;
		_validator = validator;
	}

	public async Task<InventoryUploadTrackingObject> Handle(AddAttributesCommand request, CancellationToken cancellationToken)
	{
		var groupedCategoryAttributes = request.Attributes.GroupBy(a => a.CategoryId);
		foreach (var requestAttribute in request.Attributes)
		{
			var isMultipleVarianter = groupedCategoryAttributes.Any(group => group.Key == requestAttribute.CategoryId && group.Count(a => a.Varianter) > 1);
			var isMultipleGrouped = groupedCategoryAttributes.Any(group => group.Key == requestAttribute.CategoryId && group.Count(a => a.Grouped.HasValue && a.Grouped.Value) > 1);

			if (isMultipleVarianter || isMultipleGrouped)
			{
				string message = isMultipleVarianter
					? $"Cannot add multiple varianter attributes to the same category. [CategoryId: {requestAttribute.CategoryId}] [AttributeName: {requestAttribute.Name}]"
					: $"Cannot add multiple grouped attributes to the same category. [CategoryId: {requestAttribute.CategoryId}] [AttributeName: {requestAttribute.Name}]";

				base.AddInventorUploadItem(requestAttribute, new Dictionary<string, string[]> { { "VarinterOrGrouped", new string[] { message } } });
				continue;
			}

			ValidationResult result = await _validator.ValidateAsync(requestAttribute);
			if (!result.IsValid)
			{
				base.AddInventorUploadItem(request: requestAttribute, errors: result.FormatValidationResult());
				continue;
			}

			CategoryAttribute categoryAttribute = new CategoryAttribute()
			{
				CategoryId = requestAttribute.CategoryId,
				Name = requestAttribute.Name,
				Varianter = requestAttribute.Varianter,
				Grouped = requestAttribute.Grouped,
			};

			await _eFCategoryAttributeRepository.AddAsync(categoryAttribute, cancellationToken);
			await _eFCategoryAttributeRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

			base.AddInventorUploadItem(request: requestAttribute, identifier: categoryAttribute.Id);
		}
		return base.InventoryUploadTrackingObject;
	}
}

#region Old
//var multipleVarianterCategoryIds = request.Attributes
//	.GroupBy(a => a.CategoryId)
//	.Where(group => group.Count(a => a.Varianter) > 1)
//	.Select(group => group.Key);

//var multipleGroupedCategoryIds = request.Attributes
//   .GroupBy(a => a.CategoryId)
//   .Where(group => group.Count(a => a.Grouped.HasValue && a.Grouped.Value == true) > 1)
//   .Select(group=> group.Key);

#endregion
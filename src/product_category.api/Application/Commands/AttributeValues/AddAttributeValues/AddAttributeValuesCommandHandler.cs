using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;

namespace product_category.api.Application.Commands.AttributeValues.AddAttributeValues;

public class AddAttributeValuesCommandHandler : CommandHandler, IRequestHandler<AddAttributeValuesCommand, InventoryUploadTrackingObject>
{
	private readonly IEFCategoryAttributeValueRepository _eFCategoryAttributeValueRepository;
	private readonly IValidator<AddAttributeValueRequest> _validator;

	public AddAttributeValuesCommandHandler(IEFCategoryAttributeValueRepository eFCategoryAttributeValueRepository, IValidator<AddAttributeValueRequest> validator)
	{
		_eFCategoryAttributeValueRepository = eFCategoryAttributeValueRepository;
		_validator = validator;
	}

	public async Task<InventoryUploadTrackingObject> Handle(AddAttributeValuesCommand request, CancellationToken cancellationToken)
	{
		foreach (var requestAttributeValue in request.AttributeValues)
		{
			ValidationResult result = await _validator.ValidateAsync(requestAttributeValue);
			if (!result.IsValid)
			{
				base.AddInventorUploadItem(request: requestAttributeValue, errors: result.FormatValidationResult());
				continue;
			}

			CategoryAttributeValue categoryAttributeValue = new CategoryAttributeValue()
			{
				CategoryAttributeId = requestAttributeValue.AttributeId,
				Name = requestAttributeValue.Name
			};

			await _eFCategoryAttributeValueRepository.AddAsync(categoryAttributeValue, cancellationToken);
			await _eFCategoryAttributeValueRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
			base.AddInventorUploadItem(request: requestAttributeValue, identifier: categoryAttributeValue.Id);
		}
		return base.InventoryUploadTrackingObject;
	}
}

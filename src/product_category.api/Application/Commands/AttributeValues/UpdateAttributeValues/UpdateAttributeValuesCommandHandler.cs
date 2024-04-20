using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;

namespace product_category.api.Application.Commands.AttributeValues.UpdateAttributeValues;

public class UpdateAttributeValuesCommandHandler : CommandHandler, IRequestHandler<UpdateAttributeValuesCommand, InventoryUploadTrackingObject>
{
	private readonly IEFCategoryAttributeValueRepository _eFCategoryAttributeValueRepository;
	private readonly IValidator<UpdateAttributeValueRequest> _validator;

	public UpdateAttributeValuesCommandHandler(IEFCategoryAttributeValueRepository eFCategoryAttributeValueRepository, IValidator<UpdateAttributeValueRequest> validator)
	{
		_eFCategoryAttributeValueRepository = eFCategoryAttributeValueRepository;
		_validator = validator;
	}

	public async Task<InventoryUploadTrackingObject> Handle(UpdateAttributeValuesCommand request, CancellationToken cancellationToken)
	{
		foreach (var requestAttributeValue in request.AttributeValues)
		{
			ValidationResult result = await _validator.ValidateAsync(requestAttributeValue);
			if (!result.IsValid)
			{
				base.AddInventorUploadItem(request: requestAttributeValue, errors: result.FormatValidationResult());
				continue;
			}

			CategoryAttributeValue categoryAttributeValue = (await _eFCategoryAttributeValueRepository.GetByIdAsync(requestAttributeValue.AttributeValueId, cancellationToken))!;
			categoryAttributeValue.Name = requestAttributeValue.Name;
			await _eFCategoryAttributeValueRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
			base.AddInventorUploadItem(request: requestAttributeValue, identifier: categoryAttributeValue.Id);
		}

		return base.InventoryUploadTrackingObject;
	}
}

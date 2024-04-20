using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;

namespace product_category.api.Application.Commands.AttributeValues.DeleteAttributeValues;

public class DeleteAttributeValuesCommandHandler : CommandHandler, IRequestHandler<DeleteAttributeValuesCommand, InventoryUploadTrackingObject>
{
	private readonly IEFCategoryAttributeValueRepository _eFCategoryAttributeValueRepository;
	private readonly IValidator<DeleteAttributeValueRequest> _validator;

	public DeleteAttributeValuesCommandHandler(IEFCategoryAttributeValueRepository eFCategoryAttributeValueRepository, IValidator<DeleteAttributeValueRequest> validator)
	{
		_eFCategoryAttributeValueRepository = eFCategoryAttributeValueRepository;
		_validator = validator;
	}

	public async Task<InventoryUploadTrackingObject> Handle(DeleteAttributeValuesCommand request, CancellationToken cancellationToken)
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

			_eFCategoryAttributeValueRepository.Remove(categoryAttributeValue);
			await _eFCategoryAttributeValueRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
			base.AddInventorUploadItem(request: requestAttributeValue, identifier: categoryAttributeValue.Id);
		}
		return base.InventoryUploadTrackingObject;
		throw new NotImplementedException();
	}
}

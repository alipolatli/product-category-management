using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;

namespace product_category.api.Application.Commands.Attributes.DeleteAttributes;

public class DeleteAttributesCommandHandler : CommandHandler, IRequestHandler<DeleteAttributesCommand, InventoryUploadTrackingObject>
{
	private readonly IEFCategoryAttributeRepository _eFCategoryAttributeRepository;
	private readonly IValidator<DeleteAttributeRequest> _validator;

	public DeleteAttributesCommandHandler(IEFCategoryAttributeRepository eFCategoryAttributeRepository, IValidator<DeleteAttributeRequest> validator)
	{
		_eFCategoryAttributeRepository = eFCategoryAttributeRepository;
		_validator = validator;
	}

	public async Task<InventoryUploadTrackingObject> Handle(DeleteAttributesCommand request, CancellationToken cancellationToken)
	{
		foreach (var requestAttribute in request.Attributes)
		{
			ValidationResult result = await _validator.ValidateAsync(requestAttribute);
			if (!result.IsValid)
			{
				base.AddInventorUploadItem(request: requestAttribute, errors: result.FormatValidationResult());
				continue;
			}

			CategoryAttribute categoryAttribute = (await _eFCategoryAttributeRepository.GetByIdAsync(requestAttribute.AttributeId, cancellationToken))!;
			_eFCategoryAttributeRepository.Remove(categoryAttribute);
			await _eFCategoryAttributeRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
			base.AddInventorUploadItem(request: requestAttribute, identifier: categoryAttribute.Id);
		}
		return base.InventoryUploadTrackingObject;
	}
}

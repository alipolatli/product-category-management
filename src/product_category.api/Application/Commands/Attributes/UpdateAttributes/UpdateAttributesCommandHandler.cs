using FluentValidation;
using FluentValidation.Results;
using MediatR;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;

namespace product_category.api.Application.Commands.Attributes.UpdateAttributes;

public class UpdateAttributesCommandHandler : CommandHandler, IRequestHandler<UpdateAttributesCommand, InventoryUploadTrackingObject>
{
	private readonly IEFCategoryAttributeRepository _eFCategoryAttributeRepository;
	private readonly IValidator<UpdateAttributeRequest> _validator;
	public UpdateAttributesCommandHandler(IEFCategoryAttributeRepository eFCategoryAttributeRepository, IValidator<UpdateAttributeRequest> validator)
	{
		_eFCategoryAttributeRepository = eFCategoryAttributeRepository;
		_validator = validator;
	}

	public async Task<InventoryUploadTrackingObject> Handle(UpdateAttributesCommand request, CancellationToken cancellationToken)
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
			categoryAttribute.Name = requestAttribute.Name;
			await _eFCategoryAttributeRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
			base.AddInventorUploadItem(request: requestAttribute, identifier: categoryAttribute.Id);
		}
		return base.InventoryUploadTrackingObject;
	}
}
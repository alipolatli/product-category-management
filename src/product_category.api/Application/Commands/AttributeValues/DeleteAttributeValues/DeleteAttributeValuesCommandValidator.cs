using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.AttributeValues.DeleteAttributeValues;

public class DeleteAttributeValueRequestValidator : AbstractValidator<DeleteAttributeValueRequest>
{
	private readonly IEFCategoryAttributeValueRepository _eFCategoryAttributeValueRepository;

	public DeleteAttributeValueRequestValidator(IEFCategoryAttributeValueRepository eFCategoryAttributeValueRepository)
	{
		_eFCategoryAttributeValueRepository = eFCategoryAttributeValueRepository;

		RuleFor(av => av.CategoryId)
			.MustAsync(async (request, cid, cancellation) => await _eFCategoryAttributeValueRepository.ExistCategoryAttributeValueAsync(cid, request.AttributeId, request.AttributeValueId, cancellation));
	}
}

public class DeleteAttributeValuesCommandValidator : AbstractValidator<DeleteAttributeValuesCommand>
{
	public DeleteAttributeValuesCommandValidator()
	{
		RuleFor(av => av.AttributeValues)
		   .NotEmpty();
	}
}

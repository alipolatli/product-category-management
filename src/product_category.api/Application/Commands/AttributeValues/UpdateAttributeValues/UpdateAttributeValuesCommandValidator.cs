using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.AttributeValues.UpdateAttributeValues;

public class UpdateAttributeValueRequestValidator : AbstractValidator<UpdateAttributeValueRequest>
{
	private readonly IEFCategoryAttributeValueRepository _eFCategoryAttributeValueRepository;

	public UpdateAttributeValueRequestValidator(IEFCategoryAttributeValueRepository eFCategoryAttributeValueRepository)
	{
		_eFCategoryAttributeValueRepository = eFCategoryAttributeValueRepository;

		RuleFor(av => av.CategoryId)
			.MustAsync(async (request, cid, cancellation) => await _eFCategoryAttributeValueRepository.ExistCategoryAttributeValueAsync(cid, request.AttributeId, request.AttributeValueId, cancellation));

		RuleFor(a => a.Name)
		.Cascade(CascadeMode.Stop)
		.NotNull()
		.MaximumLength(20)
			.MustAsync(async (request, avname, cancellation) => !await _eFCategoryAttributeValueRepository.ExistCategoryAttributeValueAsync(request.CategoryId, request.AttributeId, request.AttributeValueId, avname, cancellation));
	}
}

public class UpdateAttributeValuesCommandValidator : AbstractValidator<UpdateAttributeValuesCommand>
{
	public UpdateAttributeValuesCommandValidator()
	{
		RuleFor(av => av.AttributeValues)
			.NotEmpty();
	}
}

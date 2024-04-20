using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.Attributes.DeleteAttributes;

public class DeleteAttributeRequestValidator : AbstractValidator<DeleteAttributeRequest>
{
	private readonly IEFCategoryAttributeRepository _eFCategoryAttributeRepository;

	public DeleteAttributeRequestValidator(IEFCategoryAttributeRepository eFCategoryAttributeRepository)
	{
		_eFCategoryAttributeRepository = eFCategoryAttributeRepository;

		RuleFor(a => a.AttributeId)
		.NotEmpty();

		RuleFor(a => a.AttributeId)
		.MustAsync(async (request, aid, cancellation) => await _eFCategoryAttributeRepository.AnyAsync(ca => ca.CategoryId == request.CategoryId && ca.Id == aid, cancellation));
	}
}


public class DeleteAttributesCommandValidator : AbstractValidator<DeleteAttributesCommand>
{
	public DeleteAttributesCommandValidator()
	{
		RuleFor(a => a.Attributes)
			.NotEmpty();
	}
}

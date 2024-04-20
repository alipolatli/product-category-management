using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.Attributes.UpdateAttributes;

public class UpdateAttributeRequestValidator : AbstractValidator<UpdateAttributeRequest>
{
	private readonly IEFCategoryAttributeRepository _eFCategoryAttributeRepository;

	public UpdateAttributeRequestValidator(IEFCategoryAttributeRepository eFCategoryAttributeRepository)
	{
		_eFCategoryAttributeRepository = eFCategoryAttributeRepository;

		RuleFor(a => a.AttributeId)
			.NotEmpty();

		RuleFor(a => a.AttributeId)
			.MustAsync(async (request, aid, cancellation) => await _eFCategoryAttributeRepository.AnyAsync(ca => ca.CategoryId == request.CategoryId && ca.Id == aid, cancellation));

		RuleFor(a => a.Name)
			.NotNull()
			.NotEmpty()
			.MaximumLength(20);

		//RuleFor(a => a.Name)
		//	.MaximumLength(20);

		RuleFor(a => a.Name)
			.MustAsync(async (request, n, cancellation) => !await _eFCategoryAttributeRepository.AnyAsync(a => a.CategoryId == request.CategoryId && a.Name == n, cancellation));
	}
}

public class UpdateAttributesCommandValidator : AbstractValidator<UpdateAttributesCommand>
{
	public UpdateAttributesCommandValidator()
	{
		RuleFor(a => a.Attributes)
			.NotEmpty();
	}
}

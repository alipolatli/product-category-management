using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.AttributeValues.AddAttributeValues;

public class AddAttributeValueRequestValidator : AbstractValidator<AddAttributeValueRequest>
{
	private readonly IEFCategoryAttributeRepository _eFCategoryAttributeRepository;
	private readonly IEFCategoryAttributeValueRepository _eFCategoryAttributeValueRepository;

	public AddAttributeValueRequestValidator(IEFCategoryAttributeRepository eFCategoryAttributeRepository, IEFCategoryAttributeValueRepository eFCategoryAttributeValueRepository)
	{
		_eFCategoryAttributeRepository = eFCategoryAttributeRepository;
		_eFCategoryAttributeValueRepository = eFCategoryAttributeValueRepository;

		RuleFor(av => av.CategoryId)
			.MustAsync(async (request, cid, cancellation) => await _eFCategoryAttributeRepository.AnyAsync(a => a.CategoryId == cid && a.Id == request.AttributeId, cancellation));

		RuleFor(a => a.Name)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.MaximumLength(20)
			.MustAsync(async (request, n, cancellation) => !await _eFCategoryAttributeValueRepository.AnyAsync(av => av.CategoryAttributeId == request.AttributeId && av.Name == n, cancellation));
	}
}

public class AddAttributeValuesCommandValidator : AbstractValidator<AddAttributeValuesCommand>
{
	public AddAttributeValuesCommandValidator()
	{
		RuleFor(av => av.AttributeValues)
			.NotEmpty();
	}
}
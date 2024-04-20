using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.Attributes.AddAttributes;


public class AddAttributeRequestValidator : AbstractValidator<AddAttributeRequest>
{
	private readonly IEFCategoryRepository _eFCategoryRepository;
	private readonly IEFCategoryAttributeRepository _eFCategoryAttributeRepository;

	public AddAttributeRequestValidator(IEFCategoryRepository eFCategoryRepository, IEFCategoryAttributeRepository eFCategoryAttributeRepository)
	{
		_eFCategoryRepository = eFCategoryRepository;
		_eFCategoryAttributeRepository = eFCategoryAttributeRepository;

		RuleFor(a => a.CategoryId)
			.MustAsync(async (id, cancellation) => await _eFCategoryRepository.AnyAsync(id, cancellation))
			.WithMessage("Category not found.");

		RuleFor(a => a.Name)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.MaximumLength(20)
			.MustAsync(async (request, n, cancellation) => !await _eFCategoryAttributeRepository.AnyAsync(a => a.CategoryId == request.CategoryId && a.Name == n, cancellation));

		RuleFor(a => a)
			.Must(a => !(a.Varianter && a.Grouped.HasValue))
			.WithMessage("If Varianter is true, Grouped must be null. If Grouped is true, Varianter must be false.");

		When(a => a.Varianter == true, () =>
		{
			RuleFor(a => a.Varianter)
			.MustAsync(async (request, v, cancellation) => await _eFCategoryAttributeRepository.CountOfAttributes(request.CategoryId, true, null, cancellation) == 0);
		});

		When(a => a.Grouped.HasValue && a.Grouped == true, () =>
		{
			RuleFor(c => c.Grouped)
				.MustAsync(async (request, g, cancellation) => await _eFCategoryAttributeRepository.CountOfAttributes(request.CategoryId, null, true, cancellation) == 0);
		});
	}
}

public class AddAttributesCommandValidator : AbstractValidator<AddAttributesCommand>
{
	public AddAttributesCommandValidator()
	{
		RuleFor(a => a.Attributes)
			.NotNull()
			.NotEmpty();
	}
}

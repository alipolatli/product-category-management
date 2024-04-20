using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.Categories.DeleteCategories;

public class DeleteCategoryRequestValidator : AbstractValidator<DeleteCategoryRequest>
{
	private readonly IEFCategoryRepository _eFCategoryRepository;

	public DeleteCategoryRequestValidator(IEFCategoryRepository eFCategoryRepository)
	{
		_eFCategoryRepository = eFCategoryRepository;

		RuleFor(c => c.CategoryId)
			.MustAsync(async (id, cancellation) => await _eFCategoryRepository.AnyAsync(id, cancellation))
			.WithMessage($"Category not exist.")
			.MustAsync(async (id, cancellation) => !await _eFCategoryRepository.ContainsSubCategoriesAsync(id, cancellation))
			.WithMessage($"Category should not sub category.")
			.MustAsync(async (id, cancellation) => !await _eFCategoryRepository.ContainsAttributesAsync(id, cancellation))
			.WithMessage("Category should not contain attributes.");
	}
}

public class DeleteCategoriesCommandValidator : AbstractValidator<DeleteCategoriesCommand>
{
	public DeleteCategoriesCommandValidator()
	{
		RuleFor(c => c.Categories)
			.NotNull()
			.NotEmpty();
	}
}

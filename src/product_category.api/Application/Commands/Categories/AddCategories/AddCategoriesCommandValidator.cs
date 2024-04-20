using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.Categories.AddCategories;

public class AddCategoryRequestCommandValidator : AbstractValidator<AddCategoryRequest>
{
	private readonly IEFCategoryRepository _eFCategoryRepository;

	public AddCategoryRequestCommandValidator(IEFCategoryRepository eFCategoryRepository)
	{
		_eFCategoryRepository = eFCategoryRepository;

		RuleFor(c => c.Name)
			.NotNull()
			.NotEmpty()
			.MaximumLength(50);

		When(c => c.ParentId.HasValue, () =>
		{
			RuleFor(c => c.ParentId!.Value)
				.MustAsync(async (pid, cancellation) => await _eFCategoryRepository.AnyAsync(pid, cancellation))
				.WithMessage("Parent category does not exist.")
				.MustAsync(async (pid, cancellation) => !await _eFCategoryRepository.ContainsAttributesAsync(pid, cancellation))
				.WithMessage("Parent category should not contain attributes.");
		});


		When(c => c.SubCategories != null && c.SubCategories.Any(), () =>
		{
			RuleFor(c => c.SubCategories)
				.Custom((c, context) =>
				{
					if (!ValidateSubCategories(c))
						context.AddFailure("[SubCategories] The provided data is invalid. Ensure that the name is not null or empty, does not exceed 20 characters, and the parent ID is not specified.");
				});
		});
	}

	private bool ValidateSubCategories(IEnumerable<AddCategoryRequest> subCategories)
	{
		return subCategories.All(subCategory =>
			!string.IsNullOrEmpty(subCategory.Name) &&
			subCategory.Name.Length <= 50 &&
			!subCategory.ParentId.HasValue &&
			ValidateSubCategories(subCategory.SubCategories ?? Enumerable.Empty<AddCategoryRequest>())
		);
	}

}

public class AddCategoriesCommandValidator : AbstractValidator<AddCategoriesCommand>
{
	public AddCategoriesCommandValidator()
	{
		RuleFor(c => c.Categories)
			.NotNull()
			.NotEmpty();
	}
}

using FluentValidation;
using product_category.core.Abstractions.Repositories.EFCore;

namespace product_category.api.Application.Commands.Categories.UpdateCategories
{
	public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
	{
		private readonly IEFCategoryRepository _eFCategoryRepository;

		public UpdateCategoryRequestValidator(IEFCategoryRepository eFCategoryRepository)
		{
			_eFCategoryRepository = eFCategoryRepository;

			RuleFor(c => c.CategoryId)
				.MustAsync(async (id, cancellation) => await _eFCategoryRepository.AnyAsync(id, cancellation))
				.WithMessage("Category not found.");

			RuleFor(c => c.Name)
				.MaximumLength(50)
				.When(c => c.Name != null);

			When(c => c.MoveParentCategoryId.HasValue, () =>
			{
				RuleFor(c => c.MoveParentCategoryId!.Value)
					.MustAsync(async (pid, cancellation) => await _eFCategoryRepository.AnyAsync(pid, cancellation))
					.WithMessage("Parent category does not exist.")
					.MustAsync(async (pid, cancellation) => !await _eFCategoryRepository.ContainsAttributesAsync(pid, cancellation))
					.WithMessage("Parent category should not contain attributes.");
			});
		}
	}

	public class UpdateCategoriesCommandValidator : AbstractValidator<UpdateCategoriesCommand>
	{
		public UpdateCategoriesCommandValidator()
		{
			RuleFor(c => c.Categories)
				.NotNull()
				.NotEmpty();
		}
	}
}

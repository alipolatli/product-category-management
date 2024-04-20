using FluentValidation;
using Microsoft.AspNetCore.Http.Extensions;
using product_category.core.Extensions;

namespace product_category.api.Filters;

public class FluentValidationEndpointFilter : IEndpointFilter
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<FluentValidationEndpointFilter> _logger;

	public FluentValidationEndpointFilter(IServiceProvider serviceProvider, ILogger<FluentValidationEndpointFilter> logger)
	{
		_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		_logger.LogTrace($"Validating '{context.HttpContext.Request.GetDisplayUrl()}' that has {context.Arguments.Count} arguments.");
		for (var i = 0; i < context.Arguments.Count; i++)
		{
			object? arg = context.Arguments[i];
			if (arg == null)
			{
				_logger.LogDebug($"The argument {i} was null. Skipping validation.");
				continue;
			}

			Type? validatorGenericType = typeof(IValidator<>).MakeGenericType(arg.GetType());
			IValidator? validator = _serviceProvider.GetService(validatorGenericType) as IValidator;
			if (validator is null)
			{
				_logger.LogDebug($"No validator found for argument {i}.");
				continue;
			}

			var contextGenericType = typeof(ValidationContext<>).MakeGenericType(arg.GetType());
			var validationContext = Activator.CreateInstance(contextGenericType, arg) as IValidationContext;
			var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);
			if (!result.IsValid)
			{
				_logger.LogInformation($"Validator of argument {i} found {result.Errors.Count} errors.");
				Dictionary<string, string[]> errors = result.FormatValidationResult();
				return TypedResults.ValidationProblem(errors);
			}
		}
		return await next.Invoke(context);
	}
}
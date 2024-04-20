namespace product_category.api.Filters;

public static class FluentValidationEndpointFilterExtension
{
	public static RouteHandlerBuilder AddFluentValidationFilter(this RouteHandlerBuilder builder)
	{
		builder.AddEndpointFilter<FluentValidationEndpointFilter>();
		return builder;
	}

	public static RouteGroupBuilder AddFluentValidationFilter(this RouteGroupBuilder builder)
	{
		builder.AddEndpointFilter<FluentValidationEndpointFilter>();
		return builder;
	}
}
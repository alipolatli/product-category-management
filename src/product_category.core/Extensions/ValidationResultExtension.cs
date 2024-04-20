using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace product_category.core.Extensions
{
	public static class ValidationResultExtension
	{
		public static Dictionary<string, string[]> FormatValidationResult(this ValidationResult validationResult)
		{
			Dictionary<string, string[]> errors = validationResult.Errors
								.GroupBy(vf => GetJsonPropertyName(validationResult, vf))
								.ToDictionary(
									errorGroup => errorGroup.Key,
									errorGroup => errorGroup.Select(vf => vf.ErrorMessage).ToArray()
									);
			return errors;
		}

		private static string GetJsonPropertyName(ValidationResult validationResult, ValidationFailure validationFailure)
		{
			// If it has the JsonPropertyName attribute, use it; if not, use PropertyName.
			var propertyInfo = validationResult.GetType().GetProperty(validationFailure.PropertyName);
			var jsonPropertyNameAttribute = propertyInfo?
				.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true)
				.FirstOrDefault() as JsonPropertyNameAttribute;

			return jsonPropertyNameAttribute?.Name ?? validationFailure.PropertyName;
		}
	}
}

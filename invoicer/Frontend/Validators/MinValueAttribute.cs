using System.ComponentModel.DataAnnotations;

namespace Frontend.Validators
{
	/// <summary>
	/// Fails when the value is below <paramref name="minValue"/>. The message comes from
	/// <see cref="ValidationAttribute.ErrorMessage"/> or the ErrorMessageResource* pair like on built-in attributes.
	/// </summary>
	public class MinValueAttribute<T>(T minValue) : ValidationAttribute where T : IComparable<T>
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is not T typedValue)
			{
				return new ValidationResult($"The value must be of type {typeof(T).Name}");
			}
			if (typedValue.CompareTo(minValue) >= 0)
			{
				return ValidationResult.Success;
			}
			return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
		}
	}
}

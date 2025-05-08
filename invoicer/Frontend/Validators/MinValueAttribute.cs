using System.ComponentModel.DataAnnotations;

namespace Frontend.Validators
{
	public class MinValueAttribute<T>(T minValue, string errorMessage) : ValidationAttribute where T : IComparable<T>
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
			return new ValidationResult(errorMessage);
		}
	}
}

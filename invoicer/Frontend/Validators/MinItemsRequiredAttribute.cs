using System.ComponentModel.DataAnnotations;

namespace Frontend.Validators
{
	public class MinItemsRequiredAttribute(int minCount, Type itemType) : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is not IEnumerable<object> items)
			{
				return new ValidationResult("The value must be a list or collection");
			}

			if (items.Any(item => item.GetType() != itemType))
			{
				return new ValidationResult($"All items in the collection must be of type {itemType.Name}");
			}

			if (items.Count() >= minCount)
			{
				return ValidationResult.Success;
			}

			return new ValidationResult($"At least {minCount} item is required");
		}
	}
}

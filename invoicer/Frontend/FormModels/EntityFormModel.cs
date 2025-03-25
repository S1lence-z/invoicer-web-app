using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Frontend.FormModels
{
	public class EntityFormModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Ico is required")]
		[RegularExpression(@"^\d{8}$", ErrorMessage = "Ico must be 8 digits long")]
		public string Ico { get; set; } = string.Empty;

		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Phone number is required")]
		[RegularExpression(@"^\+?(\d{1,3})\)?[-. ]?(\d{1,3})[-. ]?(\d{1,4})$", ErrorMessage = "Invalid phone number format")]
		public string PhoneNumber { get; set; } = string.Empty;

		// Bank Account
		public int BankAccountId { get; set; }

		[Required(ErrorMessage = "Account number is required")]
		[RegularExpression(@"^\d{1,}$", ErrorMessage = "Account number must be at least 1 digit long")]
		public string AccountNumber { get; set; } = string.Empty;

		[Required(ErrorMessage = "Bank code is required")]
		[RegularExpression(@"^\d{4}$", ErrorMessage = "Bank code must be 4 digits long")]
		public string BankCode { get; set; } = string.Empty;

		[Required(ErrorMessage = "Bank name is required")]
		public string BankName { get; set; } = string.Empty;

		public string IBAN { get; set; } = string.Empty;

		// Address
		public int AddressId { get; set; }

		[Required(ErrorMessage = "Street is required")]
		public string Street { get; set; } = string.Empty;

		[Required(ErrorMessage = "City is required")]
		public string City { get; set; } = string.Empty;

		[Required(ErrorMessage = "Zip code is required")]
		public int ZipCode { get; set; }

		[Required(ErrorMessage = "Country is required")]
		public string Country { get; set; } = string.Empty;

		public static EntityFormModel FromEntity(Entity entity)
		{
			return new EntityFormModel
			{
				Id = entity.Id,
				Ico = entity.Ico,
				Name = entity.Name,
				Email = entity.Email,
				PhoneNumber = entity.PhoneNumber,
				BankAccountId = entity.BankAccountId,
				AccountNumber = entity.BankAccount?.AccountNumber ?? string.Empty,
				BankCode = entity.BankAccount?.BankCode ?? string.Empty,
				BankName = entity.BankAccount?.BankName ?? string.Empty,
				IBAN = entity.BankAccount?.IBAN ?? string.Empty,
				AddressId = entity.AddressId,
				Street = entity.Address?.Street ?? string.Empty,
				City = entity.Address?.City ?? string.Empty,
				ZipCode = entity.Address?.ZipCode ?? 0,
				Country = entity.Address?.Country ?? string.Empty
			};
		}
	}
}
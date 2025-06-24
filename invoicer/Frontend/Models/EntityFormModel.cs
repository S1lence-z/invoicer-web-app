using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Frontend.Models.Base;

namespace Frontend.Models
{
	public class EntityFormModel : FormModelBase<EntityFormModel, EntityDto>
	{

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

		[Required(ErrorMessage = "Is this a client of yours?")]
		public bool IsClient { get; set; } = true;

		public int CurrentNumberingSchemeId { get; set; } = 0;

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

		protected override void LoadFromDto(EntityDto dto)
		{
			ArgumentNullException.ThrowIfNull(dto);
			Id = dto.Id;
			Ico = dto.Ico;
			Name = dto.Name;
			Email = dto.Email;
			PhoneNumber = dto.PhoneNumber;
			IsClient = dto.IsClient;
			CurrentNumberingSchemeId = dto.CurrentNumberingSchemeId;
			if (dto.BankAccount is not null)
			{
				BankAccountId = dto.BankAccount.Id;
				AccountNumber = dto.BankAccount.AccountNumber;
				BankCode = dto.BankAccount.BankCode;
				BankName = dto.BankAccount.BankName;
				IBAN = dto.BankAccount.IBAN;
			}
			if (dto.Address is not null)
			{
				AddressId = dto.Address.Id;
				Street = dto.Address.Street;
				City = dto.Address.City;
				ZipCode = dto.Address.ZipCode;
				Country = dto.Address.Country;
			}
		}

		protected override void ResetProperties()
		{
			Ico = string.Empty;
			Name = string.Empty;
			Email = string.Empty;
			PhoneNumber = string.Empty;
			IsClient = true;
			BankAccountId = 0;
			AccountNumber = string.Empty;
			BankCode = string.Empty;
			BankName = string.Empty;
			IBAN = string.Empty;
			AddressId = 0;
			Street = string.Empty;
			City = string.Empty;
			ZipCode = 0;
			Country = string.Empty;
		}

		public override EntityDto ToDto()
		{
			return new EntityDto
			{
				Id = Id,
				Ico = Ico,
				Name = Name,
				Email = Email,
				PhoneNumber = PhoneNumber,
				CurrentNumberingSchemeId = CurrentNumberingSchemeId,
				BankAccountId = BankAccountId,
				AddressId = AddressId,
				IsClient = IsClient,
				BankAccount = GetBankAccountDto(),
				Address = GetAddressDto()
			};
		}

		public BankAccountDto GetBankAccountDto()
		{
			return new BankAccountDto
			{
				Id = BankAccountId,
				AccountNumber = AccountNumber,
				BankCode = BankCode,
				BankName = BankName,
				IBAN = IBAN
			};
		}

		public AddressDto GetAddressDto()
		{
			return new AddressDto
			{
				Id = AddressId,
				Street = Street,
				City = City,
				ZipCode = ZipCode,
				Country = Country
			};
		}
	}
}
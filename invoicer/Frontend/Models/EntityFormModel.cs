using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Frontend.Localization;
using Frontend.Models.Base;

namespace Frontend.Models
{
	public class EntityFormModel : FormModelBase<EntityFormModel, EntityDto>
	{

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IcoRequired))]
		[RegularExpression(@"^\d{8}$", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IcoFormat))]
		public string Ico { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.NameRequired))]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.EmailRequired))]
		[EmailAddress(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.EmailFormat))]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.PhoneRequired))]
		[RegularExpression(@"^\+?(\d{1,3})\)?[-. ]?(\d{1,3})[-. ]?(\d{1,4})$", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.PhoneFormat))]
		public string PhoneNumber { get; set; } = string.Empty;

		// Optional: printed under the invoice items when this entity is the seller
		public string RegistrationText { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IsClientRequired))]
		public bool IsClient { get; set; } = true;

		public int CurrentNumberingSchemeId { get; set; } = 0;

		// Bank Account
		public int BankAccountId { get; set; }

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.AccountNumberRequired))]
		[RegularExpression(@"^\d{1,}$", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.AccountNumberFormat))]
		public string AccountNumber { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.BankCodeRequired))]
		[RegularExpression(@"^\d{4}$", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.BankCodeFormat))]
		public string BankCode { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.BankNameRequired))]
		public string BankName { get; set; } = string.Empty;

		public string IBAN { get; set; } = string.Empty;

		// Address
		public int AddressId { get; set; }

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StreetRequired))]
		public string Street { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CityRequired))]
		public string City { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.ZipCodeRequired))]
		public int ZipCode { get; set; }

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CountryRequired))]
		public string Country { get; set; } = string.Empty;

		protected override void LoadFromDto(EntityDto dto)
		{
			ArgumentNullException.ThrowIfNull(dto);
			Id = dto.Id;
			Ico = dto.Ico;
			Name = dto.Name;
			Email = dto.Email;
			PhoneNumber = dto.PhoneNumber;
			RegistrationText = dto.RegistrationText;
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
			RegistrationText = string.Empty;
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
				RegistrationText = RegistrationText,
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
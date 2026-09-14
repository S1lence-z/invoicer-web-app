using System.Resources;
using System.Runtime.CompilerServices;

namespace Frontend.Localization
{
	/// <summary>
	/// Strongly typed access to Resources/Localization/ValidationMessages*.resx.
	/// DataAnnotations resolve <c>ErrorMessageResourceType</c>/<c>ErrorMessageResourceName</c> through public static
	/// string properties, so every key is exposed as one; the value follows <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
	/// </summary>
	public static class ValidationMessages
	{
		private static readonly ResourceManager Resources = new("Frontend.Resources.Localization.ValidationMessages", typeof(ValidationMessages).Assembly);

		private static string Get([CallerMemberName] string key = "") => Resources.GetString(key) ?? key;

		public static string SellerRequired => Get();
		public static string BuyerRequired => Get();
		public static string InvoiceNumberRequired => Get();
		public static string IssueDateRequired => Get();
		public static string DueDateRequired => Get();
		public static string VatDateRequired => Get();
		public static string StatusRequired => Get();
		public static string StatusInvalid => Get();
		public static string CurrencyRequired => Get();
		public static string CurrencyInvalid => Get();
		public static string AtLeastOneItemRequired => Get();
		public static string UnitRequired => Get();
		public static string QuantityRequired => Get();
		public static string QuantityPositive => Get();
		public static string DescriptionRequired => Get();
		public static string UnitPriceRequired => Get();
		public static string UnitPricePositive => Get();
		public static string VatRateRequired => Get();
		public static string VatRateRange => Get();
		public static string IcoRequired => Get();
		public static string IcoFormat => Get();
		public static string NameRequired => Get();
		public static string EmailRequired => Get();
		public static string EmailFormat => Get();
		public static string PhoneRequired => Get();
		public static string PhoneFormat => Get();
		public static string IsClientRequired => Get();
		public static string AccountNumberRequired => Get();
		public static string AccountNumberFormat => Get();
		public static string BankCodeRequired => Get();
		public static string BankCodeFormat => Get();
		public static string BankNameRequired => Get();
		public static string StreetRequired => Get();
		public static string CityRequired => Get();
		public static string ZipCodeRequired => Get();
		public static string CountryRequired => Get();
		public static string InvalidInputFormat => Get();
		public static string SequencePositionInvalid => Get();
		public static string SequencePaddingRange => Get();
		public static string YearFormatInvalid => Get();
		public static string ResetFrequencyInvalid => Get();
	}
}

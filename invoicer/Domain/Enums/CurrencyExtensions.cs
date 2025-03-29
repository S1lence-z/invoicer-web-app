using System.Globalization;

namespace Domain.Enums
{
	public static class CurrencyExtensions
	{
		public static string FormatAmount(this Currency currency, decimal amount)
		{
			return currency switch
			{
				Currency.CZK => string.Format(new CultureInfo("cs-CZ"), "{0:C}", amount),
				Currency.EUR => string.Format(new CultureInfo("de-DE"), "{0:C}", amount),
				Currency.USD => string.Format(new CultureInfo("en-US"), "{0:C}", amount),
				_ => amount.ToString(CultureInfo.InvariantCulture),
			};
		}
	}
}

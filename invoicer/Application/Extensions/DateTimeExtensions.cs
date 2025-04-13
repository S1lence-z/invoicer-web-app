using Domain.Enums;

namespace Application.Extensions
{
	public static class DateTimeExtensions
	{
		public static string FormatByCurrencyLocale(this DateTime dateTime, Currency currency)
		{
			return currency switch
			{
				Currency.CZK => dateTime.ToString("dd.MM.yyyy"),
				Currency.EUR => dateTime.ToString("dd.MM.yyyy"),
				Currency.USD => dateTime.ToString("MM/dd/yyyy"),
				_ => dateTime.ToString("yyyy-MM-dd"),
			};
		}
	}
}

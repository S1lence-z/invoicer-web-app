using System.Globalization;

namespace Frontend.Extensions
{
	public static class CultureInfoExtensions
	{
		public static string FormatDisplayName(this CultureInfo culture)
		{
			return culture.Name switch
			{
				"en-US" => "English (US)",
				"cs-CZ" => "Čeština",
				_ => culture.DisplayName
			};
		}
	}
}

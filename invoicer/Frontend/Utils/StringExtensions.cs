using System.Text.RegularExpressions;

namespace Frontend.Utils
{
	public static class StringExtensions
	{
		public static string SeperateCamelCase(this string inputString)
		{
			return Regex.Replace(inputString, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}
	}
}

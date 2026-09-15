using System.Resources;
using System.Runtime.CompilerServices;

namespace Frontend.Localization
{
	/// <summary>
	/// Strongly typed access to Resources/Localization/FormDefaults*.resx.
	/// Form models are created with <c>new()</c> and cannot take an <c>IStringLocalizer</c>, so prefilled
	/// values come from here; the value follows <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
	/// </summary>
	public static class FormDefaults
	{
		private static readonly ResourceManager Resources = new("Frontend.Resources.Localization.FormDefaults", typeof(FormDefaults).Assembly);

		private static string Get([CallerMemberName] string key = "") => Resources.GetString(key) ?? key;

		public static string EntityRegistrationText => Get();
	}
}

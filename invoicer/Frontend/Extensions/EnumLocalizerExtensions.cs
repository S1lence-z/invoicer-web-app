using Frontend.Localization;
using Microsoft.Extensions.Localization;
using Shared.Extensions;

namespace Frontend.Extensions
{
	public static class EnumLocalizerExtensions
	{
		/// <summary>
		/// Display name of an enum value in the current UI culture, looked up as "&lt;EnumType&gt;.&lt;Value&gt;"
		/// in <see cref="EnumResources"/>. Falls back to the spaced English name when no translation exists.
		/// </summary>
		public static string Localize<TEnum>(this IStringLocalizer<EnumResources> localizer, TEnum value) where TEnum : struct, Enum
		{
			LocalizedString result = localizer[$"{typeof(TEnum).Name}.{value}"];
			return result.ResourceNotFound ? value.ToString().SeperateCamelCase() : result.Value;
		}
	}
}

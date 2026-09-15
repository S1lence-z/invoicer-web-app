namespace Frontend.Utils
{
	/// <summary>
	/// Normalisation applied to raw text as the user types, before it reaches a form model.
	/// </summary>
	public static class InputFormat
	{
		/// <summary>
		/// Drops every whitespace character, which also covers leading and trailing padding.
		/// Intended for identifiers that are spaced out for readability when copied from
		/// elsewhere, e.g. a company number pasted as "123 456 78".
		/// </summary>
		public static string StripWhitespace(string? value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			return string.Concat(value.Where(character => !char.IsWhiteSpace(character)));
		}
	}
}

namespace Backend.Utils.InvoicePdfGenerator
{
	public abstract class ComponentBase
	{
		protected static string GetLocalizedText(string englishText, string czechText, string languageTag)
		{
			return languageTag == "cs" ? czechText : englishText;
		}
	}
}

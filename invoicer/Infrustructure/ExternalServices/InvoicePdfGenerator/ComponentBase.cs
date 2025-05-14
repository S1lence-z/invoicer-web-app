namespace Backend.Utils.InvoicePdfGenerator
{
	public abstract class ComponentBase
	{
		protected string GetLocalizedText(string englishText, string czechText, string languageTag)
		{
			return languageTag == "cs" ? czechText : englishText;
		}
	}
}

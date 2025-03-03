namespace Frontend.Models
{
	public class NavLinkItem(string label, string urlPath, string iconClass)
	{
		public string Label { get; init; } = label;
		public string UrlPath { get; init; } = urlPath;
		public string IconClass { get; init; } = iconClass;
	}
}

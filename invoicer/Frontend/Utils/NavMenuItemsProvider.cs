namespace Frontend.Utils
{
	public class NavMenuItemsProvider(IEnumerable<NavLinkItem> items)
	{
		public IEnumerable<NavLinkItem> Items { get; private init; } = items;
	}
}

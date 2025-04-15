using Frontend.Models;

namespace Frontend.Utils
{
	public class NavMenuItemsProvider(IList<NavLinkItem> items)
	{
		public IList<NavLinkItem> Items { get; private init; } = items;
	}
}

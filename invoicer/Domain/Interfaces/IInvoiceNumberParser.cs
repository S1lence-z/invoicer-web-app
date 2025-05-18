using Domain.Models;
using Domain.Utils;

namespace Domain.Interfaces
{
	public interface IInvoiceNumberParser
	{
		bool TryParse(string invoiceNumber, NumberingScheme scheme, out ParsedInvoiceNumberComponents components, out string? errorMessage);
	}
}

using Domain.Models;

namespace Domain.Interfaces
{
	public interface IInvoiceNumberGenerator
	{
		string GenerateInvoiceNumber(NumberingScheme scheme, EntityInvoiceNumberingSchemeState state, DateTime at);
	}
}

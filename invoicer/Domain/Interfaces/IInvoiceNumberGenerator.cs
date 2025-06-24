using Domain.Models;

namespace Domain.Interfaces
{
	public interface IInvoiceNumberGenerator
	{
		InvoiceNumber Generate(NumberingScheme scheme, EntityInvoiceNumberingSchemeState currentState, DateTime generationDate);
	}
}

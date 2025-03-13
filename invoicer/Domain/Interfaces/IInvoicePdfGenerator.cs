using Domain.Models;

namespace Domain.Interfaces
{
	public interface IInvoicePdfGenerator
	{
		// TODO: even better would be to take IInvoice as the param type
		public abstract static Task<IPdfGenerationResult> ExportInvoicePdf(Invoice invoice);
	}
}

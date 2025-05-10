using Domain.Models;

namespace Application.PdfGenerator
{
	public interface IInvoicePdfGenerator
	{
		IPdfGenerationResult ExportInvoicePdf(Invoice invoice, string lang);
	}
}

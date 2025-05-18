using Application.DTOs.Pdf;
using Domain.Models;

namespace Application.ExternalServiceInterfaces
{
	public interface IInvoicePdfGenerator
	{
		IPdfGenerationResult ExportInvoicePdf(Invoice invoice, string lang);
		abstract string CreateInvoiceName(Invoice invoice, string lang);
		abstract string ExtractLanguageTag(string lang);
	}
}
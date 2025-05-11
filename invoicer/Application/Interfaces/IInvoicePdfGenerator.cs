using Domain.Models;
using Shared.Interfaces;

namespace Application.Interfaces
{
	public interface IInvoicePdfGenerator
	{
		IPdfGenerationResult ExportInvoicePdf(Invoice invoice, string lang);
	}
}
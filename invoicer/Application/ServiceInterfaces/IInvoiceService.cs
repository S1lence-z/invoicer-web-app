using Application.DTOs;
using Application.Interfaces;
using Application.DTOs.Pdf;

namespace Application.ServiceInterfaces
{
	public interface IInvoiceService : IService<int, InvoiceDto> 
	{
		public Task<IPdfGenerationResult> ExportInvoicePdfAsync(int id, string lang);

		public Task<InvoiceDto> GetNewInvoiceInformationAsync(int entityId);
	}
}

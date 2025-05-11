using Shared.DTOs;
using Shared.Interfaces;

namespace Shared.ServiceInterfaces
{
	public interface IInvoiceService : IService<int, InvoiceDto> 
	{
		public Task<IPdfGenerationResult> ExportInvoicePdfAsync(int id, string lang);

		public Task<InvoiceDto> GetNewInvoiceInformationAsync(int entityId);
	}
}

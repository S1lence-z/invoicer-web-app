using Domain.Interfaces;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
	public interface IInvoiceService : IService<int, Invoice> 
	{
		public Task<IPdfGenerationResult> ExportInvoicePdf(int id);
	}
}

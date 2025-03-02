using Domain.Interfaces;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
	public interface IInvoiceService : IService<int, Invoice> 
	{
		public Task<byte[]> ExportInvoicePdf(int id);
	}
}

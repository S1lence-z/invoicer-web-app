using Backend.Services.InvoiceGeneratorService.Models;

namespace Backend.Services.InvoiceGeneratorService
{
	public interface IInvoiceGenerator
	{
		Task<Invoice> GetInvoiceById(InvoiceKey key);
		Task<IEnumerable<Invoice>> GetAllInvoices();
		Task CreateInvoice(Invoice invoice);
		Task UpdateInvoice(InvoiceKey key, Invoice invoice);
		Task DeleteInvoice(InvoiceKey key);
	}
}

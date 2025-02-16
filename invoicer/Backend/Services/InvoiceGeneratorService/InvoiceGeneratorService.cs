using Backend.Database;
using Backend.Services.InvoiceGeneratorService.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.InvoiceGeneratorService
{
	public class InvoiceGeneratorService(ApplicationDbContext context) : IInvoiceGenerator
	{
		public async Task<Invoice> GetInvoiceById(InvoiceKey id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Invoice>> GetAllInvoices()
		{
			throw new NotImplementedException();
		}

		public async Task CreateInvoice(Invoice invoice)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateInvoice(InvoiceKey key, Invoice invoice)
		{
			throw new NotImplementedException();
		}

		public async Task DeleteInvoice(InvoiceKey key)
		{
			throw new NotImplementedException();
		}
	}
}

using Backend.Database;
using Backend.Services.InvoiceService.Models;

namespace Backend.Services.InvoiceService
{
	public class InvoiceService(ApplicationDbContext context) : IInvoiceService
	{
		public Task<Invoice> CreateAsync(Invoice obj)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IList<Invoice>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Invoice?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Invoice> UpdateAsync(int id, Invoice obj)
		{
			throw new NotImplementedException();
		}
	}
}

using Application.Interfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IInvoiceItemRepository : IRepository
	{
		Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(int invoiceId, bool isReadonly);
		Task AddAsync(InvoiceItem item);
		void Update(InvoiceItem item);
		void Remove(InvoiceItem item);
	}
}

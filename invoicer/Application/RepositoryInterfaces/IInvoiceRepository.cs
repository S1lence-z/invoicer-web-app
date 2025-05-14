using Application.Interfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IInvoiceRepository : IRepository
	{
		Task<Invoice> GetByIdAsync(int id, bool isReadonly);
		Task<Invoice> CreateAsync(Invoice invoice);
		Invoice Update(Invoice invoice);
		Task<bool> DeleteAsync(int id);
		Task<IEnumerable<Invoice>> GetAllAsync();
		Task<bool> ExistsForEntityByInvoiceNumber(string invoiceNumber, int entityId, bool isReadonly);
		Task<Invoice?> GetLastInvoiceAsync(int entityId, bool isReadonly, params int[] invoiceIdsToExclude);
		Task<int> GetInvoiceCountByEntityId(int entityId);
	}
}

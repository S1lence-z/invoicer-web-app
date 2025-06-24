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
		Task<Invoice?> GetLastInvoiceByIssueDateAsync(int entityId, bool isReadonly, params int[] invoiceIdsToExclude);
		Task<int> GetInvoiceCountByEntityId(int entityId);
		Task<IEnumerable<Invoice>> GetAllInvoicesBySellerId(int entityId, bool isReadonly);
		Task<bool> IsInvoiceNumberUniqueForSellerAsync(string invoiceNumber, int sellerId);
	}
}

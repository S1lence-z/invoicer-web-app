using Domain.Interfaces;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
	public interface IInvoiceNumberingService : IService<int, InvoiceNumberScheme>
	{
		// Does this method need to be async?
		Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate);

		Task<InvoiceNumberScheme?> GetByEntityId(int entityId);
	}
}

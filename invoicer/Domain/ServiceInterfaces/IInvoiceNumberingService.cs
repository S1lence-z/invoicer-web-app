using Domain.Interfaces;
using Domain.Models;

namespace Domain.ServiceInterfaces
{
	public interface IInvoiceNumberingService : IService<int, InvoiceNumberScheme>
	{
		Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate);

		Task<InvoiceNumberScheme> GetDefaultNumberScheme();
	}
}

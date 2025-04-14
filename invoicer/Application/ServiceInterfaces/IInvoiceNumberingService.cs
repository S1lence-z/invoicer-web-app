using Application.DTOs;
using Domain.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IInvoiceNumberingService : IService<int, InvoiceNumberSchemeDto>
	{
		Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate);

		Task<InvoiceNumberSchemeDto> GetDefaultNumberScheme();
	}
}

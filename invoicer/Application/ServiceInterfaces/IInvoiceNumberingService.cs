using Application.DTOs;
using Domain.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IInvoiceNumberingService : IService<int, NumberingSchemeDto>
	{
		Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate);

		Task<NumberingSchemeDto> GetDefaultNumberScheme();
	}
}

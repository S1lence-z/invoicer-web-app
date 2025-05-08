using Domain.Models;
using Application.Enums;

namespace Application.ServiceInterfaces
{
	public interface IEntityInvoiceNumberingStateService
	{
		Task<EntityInvoiceNumberingSchemeState> CreateByEntityId(int entityId);

		Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate);

		Task<string> PeekNextInvoiceNumberAsync(int entityId, DateTime generationDate);

		Task<bool> UpdateForNextAsync(int entityId, EntityInvoiceNumberingStateUpdateStatus updateStatus, bool isUsingUserDefinedState, Invoice invoice);
	}
}

using Domain.Models;
using Domain.Enums;

namespace Application.ServiceInterfaces
{
	public interface IEntityInvoiceNumberingStateService
	{
		Task<EntityInvoiceNumberingSchemeState> CreateByEntityId(int entityId);

		Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate);

		Task<bool> UpdateForNextAsync(int entityId, EntityInvoiceNumberingStateUpdateStatus updateStatus, bool isUsingUserDefinedState, Invoice invoice);
	}
}

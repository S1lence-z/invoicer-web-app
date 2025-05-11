using Shared.Enums;
using Domain.Models;

namespace Application.Interfaces
{
	public interface IEntityInvoiceNumberingStateService
	{
		Task<EntityInvoiceNumberingSchemeState> CreateByEntityId(int entityId);

		Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate);

		Task<bool> UpdateForNextAsync(int entityId, EntityInvoiceNumberingStateUpdateStatus updateStatus, bool isUsingUserDefinedState, Invoice invoice);
	}
}

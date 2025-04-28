using Domain.Models;

namespace Application.ServiceInterfaces
{
	// TODO: maybe in the future it could implement IService<int, EntityInvoiceNumberingSchemeState>
	public interface IEntityInvoiceNumberingSchemeState
	{
		Task<EntityInvoiceNumberingSchemeState?> CreateByEntityId(int entityId);
	}
}

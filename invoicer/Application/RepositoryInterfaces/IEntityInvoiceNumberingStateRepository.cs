using Application.Interfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IEntityInvoiceNumberingStateRepository : IRepository
	{
		Task<EntityInvoiceNumberingSchemeState> GetByEntityIdAsync(int entityId, bool isReadonly);
		Task AddAsync(EntityInvoiceNumberingSchemeState state);
		void Update(EntityInvoiceNumberingSchemeState state);
		void Remove(EntityInvoiceNumberingSchemeState state);
	}
}

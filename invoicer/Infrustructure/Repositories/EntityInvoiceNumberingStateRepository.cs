using Application.RepositoryInterfaces;
using Domain.Models;
using Infrustructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrustructure.Repositories
{
	public class EntityInvoiceNumberingStateRepository(ApplicationDbContext context) : IEntityInvoiceNumberingStateRepository
	{
		public async Task AddAsync(EntityInvoiceNumberingSchemeState state)
		{
			await context.EntityInvoiceNumberingSchemeState.AddAsync(state);
		}

		public async Task<EntityInvoiceNumberingSchemeState> GetByEntityIdAsync(int entityId, bool isReadonly)
		{
			if (isReadonly)
				return await context.EntityInvoiceNumberingSchemeState
					.AsNoTracking()
					.FirstOrDefaultAsync(s => s.EntityId == entityId) ?? throw new KeyNotFoundException($"Numbering state for entity with id {entityId} not found");
			else
				return await context.EntityInvoiceNumberingSchemeState
					.FirstOrDefaultAsync(s => s.EntityId == entityId) ?? throw new KeyNotFoundException($"Numbering state for entity with id {entityId} not found");
		}

		public void Remove(EntityInvoiceNumberingSchemeState state)
		{
			context.EntityInvoiceNumberingSchemeState.Remove(state);
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}

		public void Update(EntityInvoiceNumberingSchemeState state)
		{
			context.EntityInvoiceNumberingSchemeState.Update(state);
		}
	}
}

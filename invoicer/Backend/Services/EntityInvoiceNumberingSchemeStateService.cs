using Application.ServiceInterfaces;
using Backend.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
	public class EntityInvoiceNumberingSchemeStateService(ApplicationDbContext context) : IEntityInvoiceNumberingSchemeState
	{
		public async Task<EntityInvoiceNumberingSchemeState> CreateByEntityId(int entityId)
		{
			// Check if the entity exists
			Entity? entity = await context.Entity
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == entityId);
			if (entity == null)
				throw new KeyNotFoundException($"Entity with ID {entityId} not found.");

			// Check if the numbering scheme state already exists
			EntityInvoiceNumberingSchemeState? state = await context.EntityInvoiceNumberingSchemeState
				.AsNoTracking()
				.FirstOrDefaultAsync(s => s.EntityId == entityId);
			if (state is not null)
				return state;

			// Create a new numbering scheme state
			var numberingSchemeState = new EntityInvoiceNumberingSchemeState()
			{
				EntityId = entityId
			};
			await context.EntityInvoiceNumberingSchemeState.AddAsync(numberingSchemeState);
			await context.SaveChangesAsync();
			return numberingSchemeState;
		}
	}
}

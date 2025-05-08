using Application.ServiceInterfaces;
using Backend.Database;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
	public class EntityInvoiceNumberingStateService(ApplicationDbContext context, IInvoiceNumberGenerator invoiceNumberGenerator) : IEntityInvoiceNumberingStateService
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
			EntityInvoiceNumberingSchemeState numberingSchemeState = new()
			{
				EntityId = entityId
			};
			await context.EntityInvoiceNumberingSchemeState.AddAsync(numberingSchemeState);
			await context.SaveChangesAsync();
			return numberingSchemeState;
		}

		public async Task<string> PeekNextInvoiceNumberAsync(int entityId, DateTime generationDate)
		{
			// Get the entity
			Entity? existingEntity = await context.Entity
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == entityId);
			if (existingEntity is null)
				throw new ArgumentException($"Entity with id {entityId} not found");

			// Get the numbering scheme
			int numberingSchemeId = existingEntity.CurrentNumberingSchemeId;
			NumberingScheme? numberingScheme = await context.NumberingScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == numberingSchemeId);
			if (numberingScheme is null)
				throw new ArgumentException($"Invoice Numbering Scheme with id {numberingSchemeId} not found");

			// Get current state
			EntityInvoiceNumberingSchemeState? state = await context.EntityInvoiceNumberingSchemeState
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.EntityId == entityId);
			if (state is null)
				throw new ArgumentException($"Entity Invoice Numbering Scheme State with entity id {entityId} not found");

			// Generate the next invoice number
			string newInvoiceNumber = invoiceNumberGenerator.GenerateInvoiceNumber(numberingScheme, state, generationDate);
			return newInvoiceNumber;
		}

		public async Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate)
		{
			// Get the entity
			Entity? existingEntity = await context.Entity
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == entityId);

			if (existingEntity is null)
				throw new ArgumentException($"Entity with id {entityId} not found");

			// Get the numbering scheme
			int numberingSchemeId = existingEntity.CurrentNumberingSchemeId;
			NumberingScheme? numberingScheme = await context.NumberingScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == numberingSchemeId);

			if (numberingScheme is null)
				throw new ArgumentException($"Invoice Numbering Scheme with id {numberingSchemeId} not found");

			// Get the state
			EntityInvoiceNumberingSchemeState? state = await context.EntityInvoiceNumberingSchemeState
				.FirstOrDefaultAsync(ins => ins.EntityId == entityId);

			if (state is null)
			{
				// Create a new state if it doesn't exist
				state = new EntityInvoiceNumberingSchemeState
				{
					EntityId = entityId
				};
				await context.EntityInvoiceNumberingSchemeState.AddAsync(state);
			}

			// Generate the next invoice number and update the state
			string newInvoiceNumber = invoiceNumberGenerator.GenerateInvoiceNumber(numberingScheme, state, generationDate);
			// TODO: should this be done here?
			state.UpdateForNext();

			// Save the updated state
			await context.SaveChangesAsync();
			// Return the new invoice number
			return newInvoiceNumber;
		}
	}
}

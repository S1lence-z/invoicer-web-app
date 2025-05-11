using Backend.Database;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Utils;
using Shared.Enums;
using Application.Interfaces;

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

			// Get current state
			EntityInvoiceNumberingSchemeState? state = await context.EntityInvoiceNumberingSchemeState
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.EntityId == entityId);
			if (state is null)
				throw new ArgumentException($"Entity Invoice Numbering Scheme State for entity id {entityId} not found");

			// Generate the next newInvoice number and update the state
			string newInvoiceNumber = invoiceNumberGenerator.GenerateInvoiceNumber(numberingScheme, state, generationDate);
			return newInvoiceNumber;
		}

		private async Task HandleCreatingOrUpdatingAsync(EntityInvoiceNumberingSchemeState state, bool isUsingUserDefinedState, Invoice newInvoice, NumberingScheme numberingScheme)
		{
			if (isUsingUserDefinedState)
			{
				string customSequenceNumber = InvoiceNumberUtils.ExtractSequenceNumber(newInvoice.InvoiceNumber, numberingScheme);
				if (!int.TryParse(customSequenceNumber, out int newSequenceNumber))
					throw new ArgumentException($"Invalid sequence number in invoice number: {newInvoice.InvoiceNumber}");

				// Check if the new sequence number does not already exist by looking at existing invoices
				Invoice? existingInvoice = await context.Invoice
					.AsNoTracking()
					.FirstOrDefaultAsync(i => i.InvoiceNumber == newInvoice.InvoiceNumber);
				if (existingInvoice is not null)
					throw new ArgumentException($"Invoice number {newInvoice.InvoiceNumber} already exists for this entity");

				EntityInvoiceNumberingStateUpdater.SetNewSequenceNumber(state, newSequenceNumber);
			}
			else
			{
				EntityInvoiceNumberingStateUpdater.SetNewSequenceNumber(state, state.LastSequenceNumber + 1);
			}
		}

		private async Task HandleDeletingAsync(EntityInvoiceNumberingSchemeState state, int entityId, Invoice newInvoice, NumberingScheme numberingScheme)
		{
			// Find the last invoice for this entity excluding the one being deleted
			Invoice? lastInvoice = await context.Invoice
				.AsNoTracking()
				.Where(i => i.SellerId == entityId)
				.Where(i => i.Id != newInvoice.Id)
				.OrderByDescending(i => i.Id)
				.FirstOrDefaultAsync();

			if (lastInvoice is null)
			{
				EntityInvoiceNumberingStateUpdater.SetNewSequenceNumber(state, 0);
				return;
			}

			// Extract the sequence number from the last invoice
			string extractedSequenceNumber = InvoiceNumberUtils.ExtractSequenceNumber(lastInvoice.InvoiceNumber, numberingScheme);
			if (!int.TryParse(extractedSequenceNumber, out int lastSequenceNumber))
				throw new ArgumentException($"Invalid sequence number in invoice number: {lastInvoice.InvoiceNumber}");

			// Check how many invoices this entity has
			int invoiceCount = await context.Invoice
				.AsNoTracking()
				.CountAsync(i => i.SellerId == entityId);
			EntityInvoiceNumberingStateUpdater.SetNewSequenceNumber(state, lastSequenceNumber);
		}

		public async Task<bool> UpdateForNextAsync(int entityId, EntityInvoiceNumberingStateUpdateStatus updateStatus, bool isUsingUserDefinedState, Invoice newInvoice)
		{
			// Get the entity
			Entity? existingEntity = context.Entity
				.AsNoTracking()
				.FirstOrDefault(e => e.Id == entityId);
			if (existingEntity is null)
				throw new ArgumentException($"Entity with id {entityId} not found");

			// Get the entity's numbering scheme
			int entityNumberingSchemeId = existingEntity.CurrentNumberingSchemeId;
			NumberingScheme? entityNumberingScheme = context.NumberingScheme
				.AsNoTracking()
				.FirstOrDefault(ins => ins.Id == entityNumberingSchemeId);
			if (entityNumberingScheme is null)
				throw new ArgumentException($"Invoice Numbering Scheme with id {entityNumberingSchemeId} not found");

			// Get the invoice numbering scheme
			int invoiceNumberingSchemeId = newInvoice.NumberingSchemeId;
			NumberingScheme? invoiceNumberingScheme = context.NumberingScheme
				.AsNoTracking()
				.FirstOrDefault(ins => ins.Id == invoiceNumberingSchemeId);
			if (invoiceNumberingScheme is null)
				throw new ArgumentException($"Invoice Numbering Scheme with id {invoiceNumberingSchemeId} not found");

			// Get the state
			EntityInvoiceNumberingSchemeState? state = context.EntityInvoiceNumberingSchemeState
				.FirstOrDefault(ins => ins.EntityId == entityId);
			if (state is null)
				throw new ArgumentException($"Entity Invoice Numbering Scheme State with entity id {entityId} not found");

			switch (updateStatus)
			{
				case EntityInvoiceNumberingStateUpdateStatus.Creating:
					await HandleCreatingOrUpdatingAsync(state, isUsingUserDefinedState, newInvoice, entityNumberingScheme);
					break;
				case EntityInvoiceNumberingStateUpdateStatus.Updating:
					await HandleCreatingOrUpdatingAsync(state, isUsingUserDefinedState, newInvoice, invoiceNumberingScheme);
					break;
				case EntityInvoiceNumberingStateUpdateStatus.Deleting:
					Invoice invoiceBeingDeleted = newInvoice;
					await HandleDeletingAsync(state, entityId, invoiceBeingDeleted, invoiceNumberingScheme);
					break;
				default:
					throw new ArgumentException($"Invalid update status: {updateStatus}");
			}

			await context.SaveChangesAsync();
			return true;
		}
	}
}

using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Utils;
using Shared.Enums;
using Application.Interfaces;
using Application.RepositoryInterfaces;

namespace Backend.Services
{
	public class EntityInvoiceNumberingStateService(IEntityInvoiceNumberingStateRepository numberingStateRepository, IEntityRepository entityRepository, INumberingSchemeRepository numberingSchemeRepository, IInvoiceRepository invoiceRepository, IInvoiceNumberGenerator invoiceNumberGenerator) : IEntityInvoiceNumberingStateService
	{
		public async Task<EntityInvoiceNumberingSchemeState> CreateByEntityId(int entityId)
		{
			// Check if the entity exists
			await entityRepository.GetByIdAsync(entityId, true);

			// Check if the numbering scheme state already exists
			await numberingStateRepository.GetByEntityIdAsync(entityId, false);

			// Create a new numbering scheme state
			EntityInvoiceNumberingSchemeState newNumberingState = new()
			{
				EntityId = entityId
			};
			await numberingStateRepository.AddAsync(newNumberingState);
			await numberingStateRepository.SaveChangesAsync();
			return newNumberingState;
		}

		public async Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate)
		{
			// Get the entity
			Entity existingEntity = await entityRepository.GetByIdAsync(entityId, true);
			int numberingSchemeId = existingEntity.CurrentNumberingSchemeId;

			// Get the numbering scheme
			NumberingScheme numberingScheme = await numberingSchemeRepository.GetByIdAsync(numberingSchemeId, true);

			// Get current state
			EntityInvoiceNumberingSchemeState? state = await numberingStateRepository.GetByEntityIdAsync(entityId, true);

			// Generate the next newInvoice number and update the state
			string newInvoiceNumber = invoiceNumberGenerator.GenerateInvoiceNumber(numberingScheme, state, generationDate);
			return newInvoiceNumber;
		}

		public async Task<bool> UpdateForNextAsync(int entityId, EntityInvoiceNumberingStateUpdateStatus updateStatus, bool isUsingUserDefinedState, Invoice newInvoice)
		{
			// Get the entity
			Entity existingEntity = await entityRepository.GetByIdAsync(entityId, true);

			// Get the entity's numbering scheme
			int entityNumberingSchemeId = existingEntity.CurrentNumberingSchemeId;
			NumberingScheme entityNumberingScheme = await numberingSchemeRepository.GetByIdAsync(entityNumberingSchemeId, true);

			// Get the state
			EntityInvoiceNumberingSchemeState? state = await numberingStateRepository.GetByEntityIdAsync(entityId, false);

			switch (updateStatus)
			{
				case EntityInvoiceNumberingStateUpdateStatus.Creating:
				case EntityInvoiceNumberingStateUpdateStatus.Updating:
					await HandleCreatingOrUpdatingAsync(state, isUsingUserDefinedState, newInvoice, entityNumberingScheme);
					break;
				case EntityInvoiceNumberingStateUpdateStatus.Deleting:
					await HandleDeletingAsync(state, entityId, newInvoice);
					break;
				default:
					throw new ArgumentException($"Invalid update status: {updateStatus}");
			}

			await numberingStateRepository.SaveChangesAsync();
			return true;
		}

		private async Task HandleCreatingOrUpdatingAsync(EntityInvoiceNumberingSchemeState state, bool isUsingUserDefinedState, Invoice newInvoice, NumberingScheme numberingScheme)
		{
			if (isUsingUserDefinedState)
			{
				string customSequenceNumber = InvoiceNumberUtils.ExtractSequenceNumber(newInvoice.InvoiceNumber, numberingScheme);
				if (!int.TryParse(customSequenceNumber, out int newSequenceNumber))
					throw new ArgumentException($"Invalid sequence number in invoice number: {newInvoice.InvoiceNumber}");

				// Check if the new sequence number does not already exist by looking at existing invoices for the current seller
				bool invNumberAlreadyExists = await invoiceRepository.ExistsForEntityByInvoiceNumber(newInvoice.InvoiceNumber, newInvoice.SellerId, false);
				if (!invNumberAlreadyExists)
					throw new ArgumentException($"Invoice number {newInvoice.InvoiceNumber} already exists for this entity");

				EntityInvoiceNumberingStateUpdater.SetNewSequenceNumber(state, newSequenceNumber);
			}
			else
				EntityInvoiceNumberingStateUpdater.SetNewSequenceNumber(state, state.LastSequenceNumber + 1);
		}

		private async Task HandleDeletingAsync(EntityInvoiceNumberingSchemeState state, int entityId, Invoice newInvoice)
		{
			// Find the last invoice for this entity excluding the one being deleted
			Invoice? lastInvoice = await invoiceRepository.GetLastInvoiceAsync(entityId, true, newInvoice.Id);
			if (lastInvoice is null)
			{
				EntityInvoiceNumberingStateUpdater.SetNewSequenceNumber(state, 0);
				return;
			}

			// Find the numbering scheme for the last invoice
			NumberingScheme lastInvoiceNumberingScheme = await numberingSchemeRepository.GetByIdAsync(lastInvoice.NumberingSchemeId, true);

			// Extract the sequence number from the last invoice
			string extractedSequenceNumber = InvoiceNumberUtils.ExtractSequenceNumber(lastInvoice.InvoiceNumber, lastInvoiceNumberingScheme);
			if (!int.TryParse(extractedSequenceNumber, out int lastSequenceNumber))
				throw new ArgumentException($"Invalid sequence number in invoice number: {lastInvoice.InvoiceNumber}");

			// Check how many invoices this entity has
			int invoiceCount = await invoiceRepository.GetInvoiceCountByEntityId(entityId);
			EntityInvoiceNumberingStateUpdater.SetNewSequenceNumber(state, lastSequenceNumber);
		}
	}
}

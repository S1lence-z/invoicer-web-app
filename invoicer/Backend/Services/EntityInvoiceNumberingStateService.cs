using Domain.Interfaces;
using Domain.Models;
using Shared.Enums;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces;
using Application.DTOs;

namespace Backend.Services
{
	public class EntityInvoiceNumberingStateService(IEntityInvoiceNumberingStateRepository numberingStateRepository, IEntityService entityService, INumberingSchemeRepository numberingSchemeRepository, IInvoiceRepository invoiceRepository, IInvoiceNumberGenerator invoiceNumberGenerator, IInvoiceNumberParser invoiceNumberParser) : IEntityInvoiceNumberingStateService
	{
		public async Task<EntityInvoiceNumberingSchemeState> CreateByEntityId(int entityId)
		{
			// Ensure the entity exists
			await entityService.GetByIdAsync(entityId);

			// Try to get existing state
			EntityInvoiceNumberingSchemeState? existingState = null;
			try
			{
				existingState = await numberingStateRepository.GetByEntityIdAsync(entityId, true);
			}
			catch (KeyNotFoundException)
			{
				// State does not exist, will create below
			}

			if (existingState is not null)
				return existingState;

			// Create new state
			var newNumberingState = new EntityInvoiceNumberingSchemeState
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
			EntityDto existingEntity = await entityService.GetByIdAsync(entityId);
			int numberingSchemeId = existingEntity.CurrentNumberingSchemeId;

			// Get the numbering scheme
			NumberingScheme numberingScheme = await numberingSchemeRepository.GetByIdAsync(numberingSchemeId, true);

			// Get current state
			EntityInvoiceNumberingSchemeState state = await numberingStateRepository.GetByEntityIdAsync(entityId, true);

			// Generate the next updatedInvoice number and update the state
			string newInvoiceNumber = invoiceNumberGenerator.Generate(numberingScheme, state, generationDate);
			return newInvoiceNumber;
		}

		public async Task<bool> UpdateForNextAsync(int entityId, Status updateStatus, bool isUsingUserDefinedState, Invoice newInvoice)
		{
			// Get the entity
			EntityDto existingEntity = await entityService.GetByIdAsync(entityId);

			// Get the entity's numbering scheme
			int entityNumberingSchemeId = existingEntity.CurrentNumberingSchemeId;
			NumberingScheme entityNumberingScheme = await numberingSchemeRepository.GetByIdAsync(entityNumberingSchemeId, true);

			// Get the invoice numbering scheme
			int invoiceNumberingSchemeId = newInvoice.NumberingSchemeId;
			NumberingScheme invoiceNumberingScheme = await numberingSchemeRepository.GetByIdAsync(invoiceNumberingSchemeId, true);

			// Get the state
			EntityInvoiceNumberingSchemeState state = await numberingStateRepository.GetByEntityIdAsync(entityId, false);

			switch (updateStatus)
			{
				case Status.Creating:
					await HandleCreatingAsync(state, isUsingUserDefinedState, newInvoice, entityNumberingScheme);
					break;
				case Status.Updating:
					await HandleUpdatingAsync(state, isUsingUserDefinedState, newInvoice, invoiceNumberingScheme);
					break;
				case Status.Deleting:
					await HandleDeletingAsync(state, entityId, newInvoice);
					break;
				default:
					throw new ArgumentException($"Invalid update status: {updateStatus}");
			}

			await numberingStateRepository.SaveChangesAsync();
			return true;
		}

		private async Task HandleCreatingAsync(EntityInvoiceNumberingSchemeState state, bool isUsingUserDefinedState, Invoice newInvoice, NumberingScheme newInvoiceNumberingScheme)
		{
			if (isUsingUserDefinedState)
			{
				InvoiceNumber customInvoiceNumber = InvoiceNumber.FromString(newInvoice.InvoiceNumber, newInvoiceNumberingScheme, invoiceNumberParser);
				int newSequenceNumber = customInvoiceNumber.GetSequenceNumberAsInt();

				// Check if the new sequence number does not already exist by looking at existing invoices for the current seller
				if (!await invoiceRepository.IsInvoiceNumberUniqueForSellerAsync(newInvoice.InvoiceNumber, newInvoice.SellerId))
					throw new ArgumentException($"Invoice number {newInvoice.InvoiceNumber} already exists for this entity");

				newSequenceNumber = await GetLastSequenceNumber(newInvoice, newSequenceNumber);
				state.SetNewSequenceNumber(newSequenceNumber);
			}
			else
				state.SetNewSequenceNumber(state.LastSequenceNumber + 1);
		}

		private async Task HandleUpdatingAsync(EntityInvoiceNumberingSchemeState state, bool isUsingUserDefinedState, Invoice updatedInvoice, NumberingScheme updatedInvoicesNumberingScheme)
		{
			if (isUsingUserDefinedState)
			{
				InvoiceNumber customInvoiceNumber = InvoiceNumber.FromString(updatedInvoice.InvoiceNumber, updatedInvoicesNumberingScheme, invoiceNumberParser);
				int newSequenceNumber = customInvoiceNumber.GetSequenceNumberAsInt();

				// Check if the new sequence number does not already exist by looking at existing invoices for the current seller
				if (!await invoiceRepository.IsInvoiceNumberUniqueForSellerAsync(updatedInvoice.InvoiceNumber, updatedInvoice.SellerId))
					throw new ArgumentException($"Invoice number {updatedInvoice.InvoiceNumber} already exists for this entity");

				// Set the biggest sequence number as the last one for the state
				newSequenceNumber = await GetLastSequenceNumber(updatedInvoice, newSequenceNumber);
				state.SetNewSequenceNumber(newSequenceNumber);
			}
			else
				state.SetNewSequenceNumber(state.LastSequenceNumber + 1);
		}

		private async Task HandleDeletingAsync(EntityInvoiceNumberingSchemeState state, int entityId, Invoice newInvoice)
		{
			// Find the last invoice for this entity excluding the one being deleted
			Invoice? lastInvoice = await invoiceRepository.GetLastInvoiceByIssueDateAsync(entityId, true, newInvoice.Id);
			if (lastInvoice is null)
			{
				state.SetNewSequenceNumber(0);
				return;
			}

			// Find the numbering scheme for the last invoice
			NumberingScheme lastInvoiceNumberingScheme = await numberingSchemeRepository.GetByIdAsync(lastInvoice.NumberingSchemeId, true);

			// Extract the sequence number from the last invoice
			int lastSequenceNumber = InvoiceNumber.FromString(lastInvoice.InvoiceNumber, lastInvoiceNumberingScheme, invoiceNumberParser).GetSequenceNumberAsInt();

			// Check how many invoices this entity has
			state.SetNewSequenceNumber(lastSequenceNumber);
		}

		private async Task<int> GetLastSequenceNumber(Invoice newInvoice, int newSequenceNumber)
		{
			// Set the biggest sequence number as the last one for the state
			IEnumerable<Invoice> existingInvoices = await invoiceRepository.GetAllInvoicesBySellerId(newInvoice.SellerId, true);
			foreach (Invoice invoice in existingInvoices.Where(inv => inv.Id != newInvoice.Id))
			{
				int existingSequenceNumber = InvoiceNumber.FromString(invoice.InvoiceNumber, invoice.InvoiceNumberingScheme, invoiceNumberParser).GetSequenceNumberAsInt();
				if (existingSequenceNumber > newSequenceNumber)
					newSequenceNumber = existingSequenceNumber;
			}

			return newSequenceNumber;
		}
	}
}

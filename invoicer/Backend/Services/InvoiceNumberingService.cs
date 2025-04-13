using Backend.Database;
using Backend.Utils;
using Domain.Models;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
	public class InvoiceNumberingService(ApplicationDbContext context) : IInvoiceNumberingService
	{
		public async Task<InvoiceNumberScheme?> GetByIdAsync(int id)
		{
			try
			{
				return await context.InvoiceNumberScheme
					.AsNoTracking()
					.FirstOrDefaultAsync(ins => ins.Id == id);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<IList<InvoiceNumberScheme>> GetAllAsync()
		{
			try
			{
				return await context.InvoiceNumberScheme
					.AsNoTracking()
					.ToListAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<InvoiceNumberScheme?> CreateAsync(InvoiceNumberScheme newInvoiceNumberScheme)
		{
			if (newInvoiceNumberScheme is null)
				throw new ArgumentNullException(nameof(newInvoiceNumberScheme));

			try
			{
				await context.InvoiceNumberScheme.AddAsync(newInvoiceNumberScheme);
				await context.SaveChangesAsync();
				return newInvoiceNumberScheme;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<InvoiceNumberScheme?> UpdateAsync(int id, InvoiceNumberScheme udpateScheme)
		{
			if (udpateScheme is null)
				throw new ArgumentNullException(nameof(udpateScheme));

			if (id != udpateScheme.Id)
				throw new ArgumentException("Id mismatch", nameof(id));

			InvoiceNumberScheme? existingScheme = await context.InvoiceNumberScheme
				.FirstOrDefaultAsync(ins => ins.Id == id);

			if (existingScheme is null)
				throw new KeyNotFoundException($"InvoiceNumberScheme with id {id} not found");

			try
			{
				// Manually update the properties
				existingScheme.Prefix = udpateScheme.Prefix;
				existingScheme.UseSeperator = udpateScheme.UseSeperator;
				existingScheme.Seperator = udpateScheme.Seperator;
				existingScheme.SequencePosition = udpateScheme.SequencePosition;
				existingScheme.SequencePadding = udpateScheme.SequencePadding;
				existingScheme.InvoiceNumberYearFormat = udpateScheme.InvoiceNumberYearFormat;
				existingScheme.IncludeMonth = udpateScheme.IncludeMonth;
				existingScheme.ResetFrequency = udpateScheme.ResetFrequency;

				// Save changes
				await context.SaveChangesAsync();
				return existingScheme;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<bool> DeleteAsync(int id)
		{
			InvoiceNumberScheme? scheme = await context.InvoiceNumberScheme
				.FirstOrDefaultAsync(ins => ins.Id == id);
			if (scheme is null)
				return false;

			try
			{
				context.InvoiceNumberScheme.Remove(scheme);
				await context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				throw;
			}
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
			int numberingSchemeId = existingEntity.InvoiceNumberSchemeId;
			InvoiceNumberScheme? numberingScheme = await context.InvoiceNumberScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == numberingSchemeId);

			if (numberingScheme is null)
				throw new ArgumentException($"Invoice Numbering Scheme with id {numberingSchemeId} not found");

			// Get the state
			EntityInvoiceNumberSchemeState? state = await context.EntityInvoiceNumberSchemeStates
				.FirstOrDefaultAsync(ins => ins.EntityId == entityId && ins.InvoiceNumberSchemeId == numberingSchemeId);

			if (state is null)
			{
				// Create a new state if it doesn't exist
				state = new EntityInvoiceNumberSchemeState
				{
					EntityId = entityId,
					InvoiceNumberSchemeId = numberingSchemeId
				};
				await context.EntityInvoiceNumberSchemeStates.AddAsync(state);
			}

			// Generate the next invoice number
			string newInvoiceNumber = InvoiceNumberGenerator.GenerateInvoiceNumber(numberingScheme, state, generationDate);
			// Update the state and save changes
			// TODO: should this be done here?
			state.UpdateForNext();
			await context.SaveChangesAsync();
			// Return the new invoice number
			return newInvoiceNumber;
		}

		public async Task<InvoiceNumberScheme> GetDefaultNumberScheme()
		{
			try
			{
				InvoiceNumberScheme? defaultScheme = await context.InvoiceNumberScheme
					.AsNoTracking()
					.FirstOrDefaultAsync(ins => ins.IsDefault);

				if (defaultScheme is null)
					throw new KeyNotFoundException("Default Invoice Numbering Scheme not found");

				return defaultScheme;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}

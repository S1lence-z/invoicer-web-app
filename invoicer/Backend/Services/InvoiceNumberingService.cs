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
					.Include(ins => ins.Entity)
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
					.Include(ins => ins.Entity)
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
				// Ensure state is reset
				newInvoiceNumberScheme.LastSequenceNumber = 0;
				newInvoiceNumberScheme.LastGenerationYear = 0;
				newInvoiceNumberScheme.LastGenerationMonth = 0;

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

		public async Task<InvoiceNumberScheme?> GetByEntityId(int entityId)
		{
			Entity? entity = await context.Entity
				.FirstOrDefaultAsync(e => e.Id == entityId);
			if (entity is null)
				throw new KeyNotFoundException($"Entity with id {entityId} not found");

			try
			{
				return await context.InvoiceNumberScheme
					.Include(ins => ins.Entity)
					.AsNoTracking()
					.FirstOrDefaultAsync(ins => ins.EntityId == entityId);
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
				throw new KeyNotFoundException($"Entity with id {entityId} not found");

			InvoiceNumberScheme? numberingScheme = await context.InvoiceNumberScheme
				.FirstOrDefaultAsync(ins => ins.EntityId == entityId);

			// If this entity does not have a numbering numberingScheme yet, create a default one
			bool isNewScheme = false;
			if (numberingScheme is null)
			{
				numberingScheme = InvoiceNumberScheme.CreateDefault(entityId);
				isNewScheme = true;
			}
			string newInvoiceNumber = InvoiceNumberGenerator.GenerateInvoiceNumber(numberingScheme, generationDate, isNewScheme);
			if (isNewScheme)
				await CreateAsync(numberingScheme);
			else
				await UpdateAsync(numberingScheme.Id, numberingScheme);
			return newInvoiceNumber;
		}
	}
}

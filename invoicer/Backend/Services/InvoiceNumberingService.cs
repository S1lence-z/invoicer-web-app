using Application.ServiceInterfaces;
using Backend.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Application.Mappers;
using Domain.Services;

namespace Backend.Services
{
	public class InvoiceNumberingService(ApplicationDbContext context, IInvoiceNumberGenerator invoiceNumberGenerator) : IInvoiceNumberingService
	{
		public async Task<NumberingSchemeDto?> GetByIdAsync(int id)
		{
			NumberingScheme? foundScheme = await context.InvoiceNumberScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == id);
			if (foundScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found");
			return InvoiceNumberSchemeMapper.MapToDto(foundScheme);
		}

		public async Task<IList<NumberingSchemeDto>> GetAllAsync()
		{
			List<NumberingScheme> allSchemes = await context.InvoiceNumberScheme
				.AsNoTracking()
				.ToListAsync();
			return allSchemes.Select(InvoiceNumberSchemeMapper.MapToDto).ToList();
		}

		public async Task<NumberingSchemeDto?> CreateAsync(NumberingSchemeDto newInvoiceNumberScheme)
		{			
			NumberingScheme scheme = InvoiceNumberSchemeMapper.MapToDomain(newInvoiceNumberScheme);
			await context.InvoiceNumberScheme.AddAsync(scheme);
			await context.SaveChangesAsync();
			return InvoiceNumberSchemeMapper.MapToDto(scheme);
		}

		public async Task<NumberingSchemeDto?> UpdateAsync(int id, NumberingSchemeDto udpateScheme)
		{
			NumberingScheme? existingScheme = await context.InvoiceNumberScheme
				.FirstOrDefaultAsync(ins => ins.Id == id);

			if (existingScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found");

			existingScheme.Prefix = udpateScheme.Prefix;
			existingScheme.UseSeperator = udpateScheme.UseSeperator;
			existingScheme.Seperator = udpateScheme.Seperator;
			existingScheme.SequencePosition = udpateScheme.SequencePosition;
			existingScheme.SequencePadding = udpateScheme.SequencePadding;
			existingScheme.InvoiceNumberYearFormat = udpateScheme.YearFormat;
			existingScheme.IncludeMonth = udpateScheme.IncludeMonth;
			existingScheme.ResetFrequency = udpateScheme.ResetFrequency;
			existingScheme.IsDefault = udpateScheme.IsDefault;

			await context.SaveChangesAsync();

			NumberingScheme? updatedScheme = await context.InvoiceNumberScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == id);

			if (updatedScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found after update");

			return InvoiceNumberSchemeMapper.MapToDto(updatedScheme);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			NumberingScheme? scheme = await context.InvoiceNumberScheme
				.FirstOrDefaultAsync(ins => ins.Id == id);
			if (scheme is null)
				return false;
			context.InvoiceNumberScheme.Remove(scheme);
			await context.SaveChangesAsync();
			return true;
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
			int numberingSchemeId = existingEntity.NumberingSchemeId;
			NumberingScheme? numberingScheme = await context.InvoiceNumberScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == numberingSchemeId);

			if (numberingScheme is null)
				throw new ArgumentException($"Invoice Numbering Scheme with id {numberingSchemeId} not found");

			// Get the state
			EntityInvoiceNumberingSchemeState? state = await context.EntityInvoiceNumberSchemeStates
				.FirstOrDefaultAsync(ins => ins.EntityId == entityId && ins.NumberingSchemeId == numberingSchemeId);

			if (state is null)
			{
				// Create a new state if it doesn't exist
				state = new EntityInvoiceNumberingSchemeState
				{
					EntityId = entityId,
					NumberingSchemeId = numberingSchemeId
				};
				await context.EntityInvoiceNumberSchemeStates.AddAsync(state);
			}

			// Generate the next invoice number
			string newInvoiceNumber = invoiceNumberGenerator.GenerateInvoiceNumber(numberingScheme, state, generationDate);
			// Update the state and save changes
			// TODO: should this be done here?
			state.UpdateForNext();
			await context.SaveChangesAsync();
			// Return the new invoice number
			return newInvoiceNumber;
		}

		public async Task<NumberingSchemeDto> GetDefaultNumberScheme()
		{
			NumberingScheme? defaultScheme = await context.InvoiceNumberScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.IsDefault);

			if (defaultScheme is null)
				throw new KeyNotFoundException("Default Invoice Numbering Scheme not found");

			return InvoiceNumberSchemeMapper.MapToDto(defaultScheme);
		}
	}
}

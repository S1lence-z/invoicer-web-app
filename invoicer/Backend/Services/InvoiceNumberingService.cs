using Application.ServiceInterfaces;
using Backend.Database;
using Backend.Utils;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Application.Mappers;

namespace Backend.Services
{
	public class InvoiceNumberingService(ApplicationDbContext context) : IInvoiceNumberingService
	{
		public async Task<InvoiceNumberSchemeDto?> GetByIdAsync(int id)
		{
			InvoiceNumberScheme? foundScheme = await context.InvoiceNumberScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == id);
			if (foundScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found");
			return InvoiceNumberSchemeMapper.MapToDto(foundScheme);
		}

		public async Task<IList<InvoiceNumberSchemeDto>> GetAllAsync()
		{
			List<InvoiceNumberScheme> allSchemes = await context.InvoiceNumberScheme
				.AsNoTracking()
				.ToListAsync();
			return allSchemes.Select(InvoiceNumberSchemeMapper.MapToDto).ToList();
		}

		public async Task<InvoiceNumberSchemeDto?> CreateAsync(InvoiceNumberSchemeDto newInvoiceNumberScheme)
		{			
			InvoiceNumberScheme scheme = InvoiceNumberSchemeMapper.MapToDomain(newInvoiceNumberScheme);
			await context.InvoiceNumberScheme.AddAsync(scheme);
			await context.SaveChangesAsync();
			return InvoiceNumberSchemeMapper.MapToDto(scheme);
		}

		public async Task<InvoiceNumberSchemeDto?> UpdateAsync(int id, InvoiceNumberSchemeDto udpateScheme)
		{
			InvoiceNumberScheme? existingScheme = await context.InvoiceNumberScheme
				.FirstOrDefaultAsync(ins => ins.Id == id);

			if (existingScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found");

			existingScheme.Prefix = udpateScheme.Prefix;
			existingScheme.UseSeperator = udpateScheme.UseSeperator;
			existingScheme.Seperator = udpateScheme.Seperator;
			existingScheme.SequencePosition = udpateScheme.SequencePosition;
			existingScheme.SequencePadding = udpateScheme.SequencePadding;
			existingScheme.InvoiceNumberYearFormat = udpateScheme.InvoiceNumberYearFormat;
			existingScheme.IncludeMonth = udpateScheme.IncludeMonth;
			existingScheme.ResetFrequency = udpateScheme.ResetFrequency;
			existingScheme.IsDefault = udpateScheme.IsDefault;

			await context.SaveChangesAsync();

			InvoiceNumberScheme? updatedScheme = await context.InvoiceNumberScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == id);

			if (updatedScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found after update");

			return InvoiceNumberSchemeMapper.MapToDto(updatedScheme);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			InvoiceNumberScheme? scheme = await context.InvoiceNumberScheme
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

		public async Task<InvoiceNumberSchemeDto> GetDefaultNumberScheme()
		{
			InvoiceNumberScheme? defaultScheme = await context.InvoiceNumberScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.IsDefault);

			if (defaultScheme is null)
				throw new KeyNotFoundException("Default Invoice Numbering Scheme not found");

			return InvoiceNumberSchemeMapper.MapToDto(defaultScheme);
		}
	}
}

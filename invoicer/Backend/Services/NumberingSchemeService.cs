using Application.ServiceInterfaces;
using Backend.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Application.Mappers;

namespace Backend.Services
{
	public class NumberingSchemeService(ApplicationDbContext context) : INumberingSchemeService
	{
		public async Task<NumberingSchemeDto> GetByIdAsync(int id)
		{
			NumberingScheme? foundScheme = await context.NumberingScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == id);
			if (foundScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found");
			return NumberingSchemeMapper.MapToDto(foundScheme);
		}

		public async Task<IList<NumberingSchemeDto>> GetAllAsync()
		{
			List<NumberingScheme> allSchemes = await context.NumberingScheme
				.AsNoTracking()
				.ToListAsync();
			return allSchemes.Select(NumberingSchemeMapper.MapToDto).ToList();
		}

		public async Task<NumberingSchemeDto> CreateAsync(NumberingSchemeDto newInvoiceNumberScheme)
		{			
			NumberingScheme scheme = NumberingSchemeMapper.MapToDomain(newInvoiceNumberScheme);
			await context.NumberingScheme.AddAsync(scheme);
			await context.SaveChangesAsync();
			return NumberingSchemeMapper.MapToDto(scheme);
		}

		public async Task<NumberingSchemeDto> UpdateAsync(int id, NumberingSchemeDto udpateScheme)
		{
			NumberingScheme? existingScheme = await context.NumberingScheme
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

			NumberingScheme? updatedScheme = await context.NumberingScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.Id == id);

			if (updatedScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found after update");

			return NumberingSchemeMapper.MapToDto(updatedScheme);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			NumberingScheme? scheme = await context.NumberingScheme
				.FirstOrDefaultAsync(ins => ins.Id == id);
			if (scheme is null)
				return false;
			context.NumberingScheme.Remove(scheme);
			await context.SaveChangesAsync();
			return true;
		}

		public async Task<NumberingSchemeDto> GetDefaultNumberingSchemeAsync()
		{
			NumberingScheme? defaultScheme = await context.NumberingScheme
				.AsNoTracking()
				.FirstOrDefaultAsync(ins => ins.IsDefault);

			if (defaultScheme is null)
				throw new KeyNotFoundException("Default Invoice Numbering Scheme not found");

			return NumberingSchemeMapper.MapToDto(defaultScheme);
		}
	}
}

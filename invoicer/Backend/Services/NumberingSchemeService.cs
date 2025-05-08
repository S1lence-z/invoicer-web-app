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

		public async Task<NumberingSchemeDto> CreateAsync(NumberingSchemeDto newNumberingSchemeDto)
		{
			NumberingScheme scheme = NumberingSchemeMapper.MapToDomain(newNumberingSchemeDto);
			await context.NumberingScheme.AddAsync(scheme);

			if (scheme.IsDefault)
				await SetDefaultSchemeAsync(scheme);

			await context.SaveChangesAsync();
			return NumberingSchemeMapper.MapToDto(scheme);
		}

		public async Task<NumberingSchemeDto> UpdateAsync(int id, NumberingSchemeDto udpatedSchemeDto)
		{
			NumberingScheme? existingScheme = await context.NumberingScheme
				.FirstOrDefaultAsync(ins => ins.Id == id);

			if (existingScheme is null)
				throw new KeyNotFoundException($"Invoice Numbering Scheme with id {id} not found");

			existingScheme.Prefix = udpatedSchemeDto.Prefix;
			existingScheme.UseSeperator = udpatedSchemeDto.UseSeperator;
			existingScheme.Seperator = udpatedSchemeDto.Seperator;
			existingScheme.SequencePosition = udpatedSchemeDto.SequencePosition;
			existingScheme.SequencePadding = udpatedSchemeDto.SequencePadding;
			existingScheme.InvoiceNumberYearFormat = udpatedSchemeDto.YearFormat;
			existingScheme.IncludeMonth = udpatedSchemeDto.IncludeMonth;
			existingScheme.ResetFrequency = udpatedSchemeDto.ResetFrequency;
			existingScheme.IsDefault = udpatedSchemeDto.IsDefault;

			if (existingScheme.IsDefault)
				await SetDefaultSchemeAsync(existingScheme);

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

			if (scheme.IsDefault)
				throw new InvalidOperationException("Cannot delete the default numbering scheme");

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

		private async Task SetDefaultSchemeAsync(NumberingScheme newDefaultScheme)
		{
			List<NumberingScheme> allDefaultSchemes = await context.NumberingScheme
				.Where(ins => ins.IsDefault)
				.ToListAsync();
			foreach (NumberingScheme scheme in allDefaultSchemes)
			{
				if (scheme.Id != newDefaultScheme.Id)
					scheme.IsDefault = false;
			}
			newDefaultScheme.IsDefault = true;
		}
	}
}

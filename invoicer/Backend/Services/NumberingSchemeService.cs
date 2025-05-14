using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Mappers;
using Shared.DTOs;
using Application.ServiceInterfaces;
using Application.RepositoryInterfaces;

namespace Backend.Services
{
	public class NumberingSchemeService(INumberingSchemeRepository numberingSchemeRepository) : INumberingSchemeService
	{
		public async Task<NumberingSchemeDto> GetByIdAsync(int id)
		{
			NumberingScheme foundScheme = await numberingSchemeRepository.GetByIdAsync(id, true);
			return NumberingSchemeMapper.MapToDto(foundScheme);
		}

		public async Task<IList<NumberingSchemeDto>> GetAllAsync()
		{
			IEnumerable<NumberingScheme> allSchemes = await numberingSchemeRepository.GetAllAsync();
			return allSchemes.Select(NumberingSchemeMapper.MapToDto).ToList();
		}

		public async Task<NumberingSchemeDto> CreateAsync(NumberingSchemeDto newNumberingSchemeDto)
		{
			NumberingScheme scheme = NumberingSchemeMapper.MapToDomain(newNumberingSchemeDto);

			if (scheme.IsDefault)
				await numberingSchemeRepository.SetDefaultSchemeAsync(scheme);

			if (scheme.SequencePadding < 1)
				throw new ArgumentException("Sequence padding must be at least 1");

			await numberingSchemeRepository.CreateAsync(scheme);
			await numberingSchemeRepository.SaveChangesAsync();

			NumberingScheme createdScheme = await numberingSchemeRepository.GetByIdAsync(scheme.Id, true);
			return NumberingSchemeMapper.MapToDto(createdScheme);
		}

		public async Task<NumberingSchemeDto> UpdateAsync(int id, NumberingSchemeDto udpatedSchemeDto)
		{
			NumberingScheme existingScheme = await numberingSchemeRepository.GetByIdAsync(id, false);
			if (udpatedSchemeDto.SequencePadding < 1)
				throw new ArgumentException("Sequence padding must be at least 1");

			bool wasDefault = existingScheme.IsDefault;
			existingScheme.Prefix = udpatedSchemeDto.Prefix;
			existingScheme.UseSeperator = udpatedSchemeDto.UseSeperator;
			existingScheme.Seperator = udpatedSchemeDto.Seperator;
			existingScheme.SequencePosition = udpatedSchemeDto.SequencePosition;
			existingScheme.SequencePadding = udpatedSchemeDto.SequencePadding;
			existingScheme.InvoiceNumberYearFormat = udpatedSchemeDto.YearFormat;
			existingScheme.IncludeMonth = udpatedSchemeDto.IncludeMonth;
			existingScheme.ResetFrequency = udpatedSchemeDto.ResetFrequency;
			existingScheme.IsDefault = udpatedSchemeDto.IsDefault;

			if (wasDefault && !existingScheme.IsDefault)
				throw new InvalidOperationException("Cannot unset the default numbering scheme. You can only set a different scheme as default.");

			if (existingScheme.IsDefault)
				await numberingSchemeRepository.SetDefaultSchemeAsync(existingScheme);

			numberingSchemeRepository.Update(existingScheme);
			await numberingSchemeRepository.SaveChangesAsync();

			NumberingScheme? updatedScheme = await numberingSchemeRepository.GetByIdAsync(id, true);
			return NumberingSchemeMapper.MapToDto(updatedScheme);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			NumberingScheme schemeToDelete = await numberingSchemeRepository.GetByIdAsync(id, false);
			if (schemeToDelete.IsDefault)
				throw new InvalidOperationException("Cannot delete the default numbering scheme");

			bool isInUseByEntity = await numberingSchemeRepository.IsInUseByEntity(schemeToDelete);
			if (isInUseByEntity)
				throw new ArgumentException("Cannot delete the scheme as it is being used by an entity");

			bool status = await numberingSchemeRepository.DeleteAsync(id);
			await numberingSchemeRepository.SaveChangesAsync();
			return status;
		}

		public async Task<NumberingSchemeDto> GetDefaultNumberingSchemeAsync()
		{
			NumberingScheme defaultScheme = await numberingSchemeRepository.GetDefaultScheme(true);
			return NumberingSchemeMapper.MapToDto(defaultScheme);
		}
	}
}

using Domain.Models;
using Shared.DTOs;

namespace Application.Mappers
{
	public class NumberingSchemeMapper
	{
		public static NumberingSchemeDto MapToDto(NumberingScheme scheme)
		{
			return new NumberingSchemeDto
			{
				Id = scheme.Id,
				Prefix = scheme.Prefix,
				UseSeperator = scheme.UseSeperator,
				Seperator = scheme.Seperator,
				SequencePosition = scheme.SequencePosition,
				SequencePadding = scheme.SequencePadding,
				YearFormat = scheme.InvoiceNumberYearFormat,
				IncludeMonth = scheme.IncludeMonth,
				ResetFrequency = scheme.ResetFrequency,
				IsDefault = scheme.IsDefault
			};
		}

		public static NumberingScheme MapToDomain(NumberingSchemeDto schemeDto)
		{
			return new NumberingScheme
			{
				Id = schemeDto.Id,
				Prefix = schemeDto.Prefix,
				UseSeperator = schemeDto.UseSeperator,
				Seperator = schemeDto.Seperator,
				SequencePosition = schemeDto.SequencePosition,
				SequencePadding = schemeDto.SequencePadding,
				InvoiceNumberYearFormat = schemeDto.YearFormat,
				IncludeMonth = schemeDto.IncludeMonth,
				ResetFrequency = schemeDto.ResetFrequency,
				IsDefault = schemeDto.IsDefault
			};
		}
	}
}

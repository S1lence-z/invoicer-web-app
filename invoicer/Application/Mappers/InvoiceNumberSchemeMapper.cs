using Application.DTOs;
using Domain.Models;

namespace Application.Mappers
{
	public class InvoiceNumberSchemeMapper
	{
		public static InvoiceNumberSchemeDto MapToDto(InvoiceNumberScheme scheme)
		{
			return new InvoiceNumberSchemeDto
			{
				Id = scheme.Id,
				Prefix = scheme.Prefix,
				UseSeperator = scheme.UseSeperator,
				Seperator = scheme.Seperator,
				SequencePosition = scheme.SequencePosition,
				SequencePadding = scheme.SequencePadding,
				InvoiceNumberYearFormat = scheme.InvoiceNumberYearFormat,
				IncludeMonth = scheme.IncludeMonth,
				ResetFrequency = scheme.ResetFrequency,
				IsDefault = scheme.IsDefault
			};
		}

		public static InvoiceNumberScheme MapToDomain(InvoiceNumberSchemeDto schemeDto)
		{
			return new InvoiceNumberScheme
			{
				Id = schemeDto.Id,
				Prefix = schemeDto.Prefix,
				UseSeperator = schemeDto.UseSeperator,
				Seperator = schemeDto.Seperator,
				SequencePosition = schemeDto.SequencePosition,
				SequencePadding = schemeDto.SequencePadding,
				InvoiceNumberYearFormat = schemeDto.InvoiceNumberYearFormat,
				IncludeMonth = schemeDto.IncludeMonth,
				ResetFrequency = schemeDto.ResetFrequency,
				IsDefault = schemeDto.IsDefault
			};
		}
	}
}

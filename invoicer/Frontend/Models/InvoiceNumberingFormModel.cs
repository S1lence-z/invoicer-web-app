using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Shared.Enums;

namespace Frontend.Models
{
	public record class InvoiceNumberingFormModel
	{
		public int Id { get; set; }

		[RegularExpression(@"^.*$", ErrorMessage = "Invalid input format")]
		public string Prefix { get; set; } = string.Empty;

		public bool UseSeperator { get; set; } = true;

		[RegularExpression(@"^.*$", ErrorMessage = "Invalid input format")]
		public string Seperator { get; set; } = "-";

		[EnumDataType(typeof(Position), ErrorMessage = "Invalid sequence position")]
		public Position SequencePosition { get; set; } = Position.Start;

		[Range(1, 10, ErrorMessage = "Sequence padding must be between 1 and 10")]
		public int SequencePadding { get; set; } = 3;

		[EnumDataType(typeof(YearFormat), ErrorMessage = "Invalid year format")]
		public YearFormat InvoiceNumberYearFormat { get; set; } = YearFormat.FourDigit;

		public bool IncludeMonth { get; set; } = true;

		[EnumDataType(typeof(ResetFrequency), ErrorMessage = "Invalid reset frequency")]
		public ResetFrequency ResetFrequency { get; set; } = ResetFrequency.Yearly;

		public bool IsDefault { get; set; } = false;

		public static InvoiceNumberingFormModel FromDto(NumberingSchemeDto dto)
		{
			return new InvoiceNumberingFormModel
			{
				Id = dto.Id,
				Prefix = dto.Prefix,
				UseSeperator = dto.UseSeperator,
				Seperator = dto.Seperator,
				SequencePosition = dto.SequencePosition,
				SequencePadding = dto.SequencePadding,
				InvoiceNumberYearFormat = dto.YearFormat,
				IncludeMonth = dto.IncludeMonth,
				ResetFrequency = dto.ResetFrequency,
				IsDefault = dto.IsDefault
			};
		}

		public NumberingSchemeDto ToDto()
		{
			return new NumberingSchemeDto
			{
				Id = Id,
				Prefix = Prefix,
				UseSeperator = UseSeperator,
				Seperator = Seperator,
				SequencePosition = SequencePosition,
				SequencePadding = SequencePadding,
				YearFormat = InvoiceNumberYearFormat,
				IncludeMonth = IncludeMonth,
				ResetFrequency = ResetFrequency,
				IsDefault = IsDefault
			};
		}
	}
}

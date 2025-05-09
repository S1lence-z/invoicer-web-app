using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Application.DTOs;

namespace Frontend.Models
{
	public record class NumberingSchemeFormModel
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
		public YearFormat YearFormat { get; set; } = YearFormat.FourDigit;

		public bool IncludeMonth { get; set; } = true;

		[EnumDataType(typeof(ResetFrequency), ErrorMessage = "Invalid reset frequency")]
		public ResetFrequency ResetFrequency { get; set; } = ResetFrequency.Yearly;

		public bool IsDefault { get; set; } = false;

		public static NumberingSchemeFormModel FromDto(NumberingSchemeDto dto)
		{
			return new NumberingSchemeFormModel
			{
				Id = dto.Id,
				Prefix = dto.Prefix,
				UseSeperator = dto.UseSeperator,
				Seperator = dto.Seperator,
				SequencePosition = dto.SequencePosition,
				SequencePadding = dto.SequencePadding,
				YearFormat = dto.YearFormat,
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
				YearFormat = YearFormat,
				IncludeMonth = IncludeMonth,
				ResetFrequency = ResetFrequency,
				IsDefault = IsDefault
			};
		}
	}
}

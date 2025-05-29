using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Frontend.Models.Base;
using Shared.Enums;

namespace Frontend.Models
{
	public class NumberingSchemeFormModel : FormModelBase<NumberingSchemeFormModel, NumberingSchemeDto>
	{
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

		public override NumberingSchemeDto ToDto()
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

		protected override void LoadFromDto(NumberingSchemeDto dto)
		{
			Id = dto.Id;
			Prefix = dto.Prefix;
			UseSeperator = dto.UseSeperator;
			Seperator = dto.Seperator;
			SequencePosition = dto.SequencePosition;
			SequencePadding = dto.SequencePadding;
			YearFormat = dto.YearFormat;
			IncludeMonth = dto.IncludeMonth;
			ResetFrequency = dto.ResetFrequency;
			IsDefault = dto.IsDefault;
		}

		protected override void ResetProperties()
		{
			Id = 0;
			Prefix = string.Empty;
			UseSeperator = true;
			Seperator = "-";
			SequencePosition = Position.Start;
			SequencePadding = 3;
			YearFormat = YearFormat.FourDigit;
			IncludeMonth = true;
			ResetFrequency = ResetFrequency.Yearly;
			IsDefault = false;
		}
	}
}

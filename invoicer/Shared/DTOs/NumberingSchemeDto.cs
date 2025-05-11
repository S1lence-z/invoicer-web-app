using Shared.Enums;

namespace Shared.DTOs
{
	public record class NumberingSchemeDto
	{
		public int Id { get; set; }
		public string Prefix { get; set; } = string.Empty;
		public bool UseSeperator { get; set; } = true;
		public string Seperator { get; set; } = "-";
		public Position SequencePosition { get; set; } = Position.Start;
		public int SequencePadding { get; set; } = 3;
		public YearFormat YearFormat { get; set; } = YearFormat.FourDigit;
		public bool IncludeMonth { get; set; } = true;
		public ResetFrequency ResetFrequency { get; set; } = ResetFrequency.Yearly;
		public bool IsDefault { get; set; } = false;
	}
}

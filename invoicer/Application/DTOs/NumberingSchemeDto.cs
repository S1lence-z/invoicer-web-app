using Domain.Enums;

namespace Application.DTOs
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

		public static string GetPreview(NumberingSchemeDto numberingScheme, int sequenceNumber)
		{
			var yearFormat = numberingScheme.YearFormat switch
			{
				YearFormat.FourDigit => DateTime.Now.ToString("yyyy"),
				YearFormat.TwoDigit => DateTime.Now.ToString("yy"),
				_ => string.Empty
			};

			var seperator = numberingScheme.UseSeperator ? numberingScheme.Seperator : string.Empty;

			var monthFormat = numberingScheme.IncludeMonth ? DateTime.Now.ToString("MM") : string.Empty;

			var sequenceFormat = numberingScheme.SequencePadding > 0
				? sequenceNumber.ToString($"D{numberingScheme.SequencePadding}")
			: sequenceNumber.ToString();

			return numberingScheme.SequencePosition switch
			{
				Position.Start => $"{numberingScheme.Prefix}{sequenceFormat}{seperator}{yearFormat}{seperator}{monthFormat}",
				Position.End => $"{numberingScheme.Prefix}{yearFormat}{seperator}{monthFormat}{seperator}{sequenceFormat}",
				_ => throw new InvalidOperationException("Invalid sequence position.")
			};
		}
	}
}

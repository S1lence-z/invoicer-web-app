using Domain.Enums;

namespace Application.DTOs
{
	public record class InvoiceNumberSchemeDto
	{
		public int Id { get; set; }
		public string Prefix { get; set; } = string.Empty;
		public bool UseSeperator { get; set; } = true;
		public string Seperator { get; set; } = "-";
		public InvoiceNumberSequencePosition SequencePosition { get; set; } = InvoiceNumberSequencePosition.Start;
		public int SequencePadding { get; set; } = 3;
		public InvoiceNumberYearFormat InvoiceNumberYearFormat { get; set; } = InvoiceNumberYearFormat.FourDigit;
		public bool IncludeMonth { get; set; } = true;
		public InvoiceNumberResetFrequency ResetFrequency { get; set; } = InvoiceNumberResetFrequency.Yearly;
		public bool IsDefault { get; set; } = false;

		public static string GetPreview(InvoiceNumberSchemeDto numberingScheme, int sequenceNumber)
		{
			var yearFormat = numberingScheme.InvoiceNumberYearFormat switch
			{
				InvoiceNumberYearFormat.FourDigit => DateTime.Now.ToString("yyyy"),
				InvoiceNumberYearFormat.TwoDigit => DateTime.Now.ToString("yy"),
				_ => string.Empty
			};

			var seperator = numberingScheme.UseSeperator ? numberingScheme.Seperator : string.Empty;

			var monthFormat = numberingScheme.IncludeMonth ? DateTime.Now.ToString("MM") : string.Empty;

			var sequenceFormat = numberingScheme.SequencePadding > 0
				? sequenceNumber.ToString($"D{numberingScheme.SequencePadding}")
			: sequenceNumber.ToString();

			return numberingScheme.SequencePosition switch
			{
				InvoiceNumberSequencePosition.Start => $"{numberingScheme.Prefix}{sequenceFormat}{seperator}{yearFormat}{seperator}{monthFormat}",
				InvoiceNumberSequencePosition.End => $"{numberingScheme.Prefix}{yearFormat}{seperator}{monthFormat}{seperator}{sequenceFormat}",
				_ => throw new InvalidOperationException("Invalid sequence position.")
			};
		}
	}
}

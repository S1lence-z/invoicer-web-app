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
	}
}

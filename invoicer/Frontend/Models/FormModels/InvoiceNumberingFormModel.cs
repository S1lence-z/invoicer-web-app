using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Application.DTOs;

namespace Frontend.Models.FormModels
{
	public class InvoiceNumberingFormModel
	{
		public int Id { get; set; }

		[RegularExpression(@"^.*$", ErrorMessage = "Invalid input format")]
		public string Prefix { get; set; } = string.Empty;

		public bool UseSeperator { get; set; } = true;

		[RegularExpression(@"^.*$", ErrorMessage = "Invalid input format")]
		public string Seperator { get; set; } = "-";

		[EnumDataType(typeof(InvoiceNumberSequencePosition), ErrorMessage = "Invalid sequence position")]
		public InvoiceNumberSequencePosition SequencePosition { get; set; } = InvoiceNumberSequencePosition.Start;

		[Range(1, 10, ErrorMessage = "Sequence padding must be between 1 and 10")]
		public int SequencePadding { get; set; } = 3;

		[EnumDataType(typeof(InvoiceNumberYearFormat), ErrorMessage = "Invalid year format")]
		public InvoiceNumberYearFormat InvoiceNumberYearFormat { get; set; } = InvoiceNumberYearFormat.FourDigit;

		public bool IncludeMonth { get; set; } = true;

		[EnumDataType(typeof(InvoiceNumberResetFrequency), ErrorMessage = "Invalid reset frequency")]
		public InvoiceNumberResetFrequency ResetFrequency { get; set; } = InvoiceNumberResetFrequency.Yearly;

		public bool IsDefault { get; set; } = false;
	}
}

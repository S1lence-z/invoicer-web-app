using Domain.Models;
using Domain.Enums;

namespace Backend.Utils
{
	public static class InvoiceNumberGenerator
	{
		public static string GenerateInvoiceNumber(InvoiceNumberScheme scheme, DateTime generationDate, bool isNewScheme)
		{
			string prefix = scheme.Prefix;
			string yearPart = GetYearPart(generationDate, scheme.InvoiceNumberYearFormat);

			// Step 2: Handle month, if applicable
			string monthPart = string.Empty;
			if (scheme.IncludeMonth)
			{
				monthPart = GetMonthPart(generationDate);
			}

			// Step 3: Handle the sequence number
			string sequenceNumber = GenerateSequenceNumber(scheme, generationDate, isNewScheme);

			// Step 4: Format the invoice number
			string invoiceNumber = FormatInvoiceNumber(scheme, prefix, yearPart, monthPart, sequenceNumber);
			
			return invoiceNumber;
		}

		private static string GetYearPart(DateTime generationDate, InvoiceNumberYearFormat yearFormat)
		{
			return yearFormat switch
			{
				InvoiceNumberYearFormat.None => string.Empty,
				InvoiceNumberYearFormat.TwoDigit => generationDate.ToString("yy"),
				InvoiceNumberYearFormat.FourDigit => generationDate.ToString("yyyy"),
				_ => throw new InvalidOperationException("Invalid year format."),
			};
		}

		private static string GetMonthPart(DateTime generationDate)
		{
			return generationDate.ToString("MM");
		}

		private static string GenerateSequenceNumber(InvoiceNumberScheme scheme, DateTime generationDate, bool isNewScheme)
		{
			// Retrieve the last sequence number and handle reset logic
			int sequence = isNewScheme ? 1 : scheme.LastSequenceNumber + 1;

			// Check for reset conditions based on the reset frequency
			if (scheme.ShouldResetSequence(generationDate))
			{
				sequence = 1;
			}

			// Format the sequence number with padding
			return sequence.ToString($"D{scheme.SequencePadding}");
		}

		private static string FormatInvoiceNumber(InvoiceNumberScheme scheme, string prefix, string yearPart, string monthPart, string sequenceNumber)
		{
			string seperator = scheme.UseSeperator ? scheme.Seperator : string.Empty;
			return scheme.SequencePosition switch
			{
				InvoiceNumberSequencePosition.Start => $"{prefix}{sequenceNumber}{seperator}{yearPart}{seperator}{monthPart}",
				InvoiceNumberSequencePosition.End => $"{prefix}{yearPart}{seperator}{monthPart}{seperator}{sequenceNumber}",
				_ => throw new InvalidOperationException("Invalid sequence position."),
			};
		}
	}
}

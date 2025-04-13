using Domain.Models;
using Domain.Enums;

namespace Backend.Utils
{
	public static class InvoiceNumberGenerator
	{
		public static string GenerateInvoiceNumber(InvoiceNumberScheme scheme, EntityInvoiceNumberSchemeState state, DateTime generationDate)
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
			string sequenceNumber = GenerateSequenceNumber(scheme, state, generationDate);

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

		private static string GenerateSequenceNumber(InvoiceNumberScheme scheme, EntityInvoiceNumberSchemeState state, DateTime generationDate)
		{
			// Check if the sequence number should be reset based on the scheme's reset frequency
			int sequenceNumber = ShouldResetSequenceNumber(scheme, state, generationDate) ? 1 : state.LastSequenceNumber + 1;
			// Format the sequence number with padding
			return sequenceNumber.ToString($"D{scheme.SequencePadding}");
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

		private static bool ShouldResetSequenceNumber(InvoiceNumberScheme scheme, EntityInvoiceNumberSchemeState state, DateTime generationDate)
		{
			return scheme.ResetFrequency switch
			{
				InvoiceNumberResetFrequency.Yearly => generationDate.Year != state.LastGenerationYear,
				InvoiceNumberResetFrequency.Monthly => generationDate.Year != state.LastGenerationYear || generationDate.Month != state.LastGenerationMonth,
				_ => false,
			};
		}
	}
}

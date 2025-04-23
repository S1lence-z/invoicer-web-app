using Domain.Models;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Services
{
	public sealed class InvoiceNumberGenerator : IInvoiceNumberGenerator
	{
		public string GenerateInvoiceNumber(NumberingScheme scheme, EntityInvoiceNumberingSchemeState state, DateTime generationDate)
		{
			string prefix = scheme.Prefix;
			string yearPart = GetYearPart(generationDate, scheme.InvoiceNumberYearFormat);

			// Handle month, if applicable
			string monthPart = string.Empty;
			if (scheme.IncludeMonth)
			{
				monthPart = GetMonthPart(generationDate);
			}

			// Step 3: Handle the sequence number
			string sequenceNumber = GenerateSequenceNumber(scheme, state, generationDate);

			// Format the invoice number
			string invoiceNumber = FormatInvoiceNumber(scheme, prefix, yearPart, monthPart, sequenceNumber);

			return invoiceNumber;
		}

		private static string GetYearPart(DateTime generationDate, YearFormat yearFormat)
		{
			return yearFormat switch
			{
				YearFormat.None => string.Empty,
				YearFormat.TwoDigit => generationDate.ToString("yy"),
				YearFormat.FourDigit => generationDate.ToString("yyyy"),
				_ => throw new InvalidOperationException("Invalid year format."),
			};
		}

		private static string GetMonthPart(DateTime generationDate)
		{
			return generationDate.ToString("MM");
		}

		private static string GenerateSequenceNumber(NumberingScheme scheme, EntityInvoiceNumberingSchemeState state, DateTime generationDate)
		{
			// Check if the sequence number should be reset based on the scheme's reset frequency
			int sequenceNumber = ShouldResetSequenceNumber(scheme, state, generationDate) ? 1 : state.LastSequenceNumber + 1;
			// Format the sequence number with padding
			return sequenceNumber.ToString($"D{scheme.SequencePadding}");
		}

		private static string FormatInvoiceNumber(NumberingScheme scheme, string prefix, string yearPart, string monthPart, string sequenceNumber)
		{
			string seperator = scheme.UseSeperator ? scheme.Seperator : string.Empty;
			return scheme.SequencePosition switch
			{
				Position.Start => $"{prefix}{sequenceNumber}{seperator}{yearPart}{seperator}{monthPart}",
				Position.End => $"{prefix}{yearPart}{seperator}{monthPart}{seperator}{sequenceNumber}",
				_ => throw new InvalidOperationException("Invalid sequence position."),
			};
		}

		private static bool ShouldResetSequenceNumber(NumberingScheme scheme, EntityInvoiceNumberingSchemeState state, DateTime generationDate)
		{
			return scheme.ResetFrequency switch
			{
				ResetFrequency.Yearly => generationDate.Year != state.LastGenerationYear,
				ResetFrequency.Monthly => generationDate.Year != state.LastGenerationYear || generationDate.Month != state.LastGenerationMonth,
				_ => false,
			};
		}
	}
}

using Domain.Models;
using Domain.Enums;
using System.Text;

namespace Backend.Utils
{
	public static class InvoiceNumberGenerator
	{
		public static string GenerateInvoiceNumber(InvoiceNumberScheme scheme, DateTime generationDate, bool isNewScheme)
		{
			// Step 1: Initialize components for the number
			string prefix = scheme.Prefix;
			string yearPart = GetYearPart(generationDate, scheme.InvoiceNumberYearFormat);

			// Step 2: Handle month, if applicable
			string monthPart = string.Empty;
			if (scheme.IncludeMonth)
			{
				monthPart = generationDate.ToString("MM"); // Always two digits
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

		private static string GenerateSequenceNumber(InvoiceNumberScheme scheme, DateTime generationDate, bool isNewScheme)
		{
			// Retrieve the last sequence number and handle reset logic
			int sequence = isNewScheme ? 1 : scheme.LastSequenceNumber + 1;

			// Check for reset conditions based on the reset frequency
			if (ShouldResetSequence(scheme, generationDate))
			{
				sequence = 1;
			}

			// Update the scheme's last sequence number
			scheme.LastSequenceNumber = sequence;

			// Format the sequence number with padding
			return sequence.ToString($"D{scheme.SequencePadding}"); // D{padding} formats with leading zeros
		}

		private static bool ShouldResetSequence(InvoiceNumberScheme scheme, DateTime generationDate)
		{
			// Reset frequency logic: monthly, yearly, etc.
			if (scheme.ResetFrequency == InvoiceNumberResetFrequency.Yearly && scheme.LastGenerationYear != generationDate.Year)
			{
				scheme.LastGenerationYear = generationDate.Year;
				return true;
			}

			if (scheme.ResetFrequency == InvoiceNumberResetFrequency.Monthly && scheme.LastGenerationMonth != generationDate.Month)
			{
				scheme.LastGenerationMonth = generationDate.Month;
				return true;
			}

			return false;
		}

		private static string FormatInvoiceNumber(InvoiceNumberScheme scheme, string prefix, string yearPart, string monthPart, string sequenceNumber)
		{
			// Format the invoice number based on the position of the sequence
			return scheme.SequencePosition switch
			{
				InvoiceNumberSequencePosition.Start => $"{prefix}{yearPart}{monthPart}-{sequenceNumber}",
				InvoiceNumberSequencePosition.End => $"{prefix}{yearPart}{monthPart}-{sequenceNumber}",
				_ => throw new InvalidOperationException("Invalid sequence position."),
			};
		}
	}
}

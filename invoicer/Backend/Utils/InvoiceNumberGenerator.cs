using Domain.Models;
using Domain.Enums;

namespace Backend.Utils
{
	/// <summary>
	/// Utility class for generating invoice numbers based on specified schemes.
	/// </summary>
	public static class InvoiceNumberGenerator
	{
		public static string GenerateInvoiceNumber(InvoiceNumberScheme scheme, EntityInvoiceNumberSchemeState state, DateTime generationDate)
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

		/// <summary>
		/// Retrieves the year part of the invoice number based on the specified year format.
		/// </summary>
		/// <param name="generationDate">The generation date to extract the year from.</param>
		/// <param name="yearFormat">The format to apply to the year.</param>
		/// <returns>A string representing the year formatted as specified.</returns>
		/// <exception cref="InvalidOperationException">Thrown if an invalid year format is provided.</exception>
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

		/// <summary>
		/// Retrieves the month part of the invoice number.
		/// </summary>
		/// <param name="generationDate">The generation date to extract the month from.</param>
		/// <returns>A string representing the month in two digits.</returns>
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

		/// <summary>
		/// Formats the invoice number components into a single invoice number string.
		/// </summary>
		/// <param name="scheme">The invoice number scheme containing formatting settings.</param>
		/// <param name="prefix">The prefix for the invoice number.</param>
		/// <param name="yearPart">The formatted year part.</param>
		/// <param name="monthPart">The formatted month part.</param>
		/// <param name="sequenceNumber">The formatted sequence number.</param>
		/// <returns>A combined string representing the complete invoice number.</returns>
		/// <exception cref="InvalidOperationException">Thrown if an invalid sequence position is specified in the scheme.</exception>
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

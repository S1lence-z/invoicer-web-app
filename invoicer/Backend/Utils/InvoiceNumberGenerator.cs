using Domain.Models;
using Domain.Enums;

namespace Backend.Utils
{
	/// <summary>
	/// Utility class for generating invoice numbers based on specified schemes.
	/// </summary>
	public static class InvoiceNumberGenerator
	{
		/// <summary>
		/// Generates an invoice number based on the provided scheme and generation date.
		/// </summary>
		/// <param name="scheme">The invoice number scheme containing formatting settings.</param>
		/// <param name="generationDate">The date for which the invoice is generated.</param>
		/// <param name="isNewScheme">Determines if a new scheme should reset the sequence number.</param>
		/// <returns>A formatted invoice number string.</returns>
		public static string GenerateInvoiceNumber(InvoiceNumberScheme scheme, DateTime generationDate, bool isNewScheme)
		{
			string prefix = scheme.Prefix;
			string yearPart = GetYearPart(generationDate, scheme.InvoiceNumberYearFormat);

			// Handle month, if applicable
			string monthPart = string.Empty;
			if (scheme.IncludeMonth)
			{
				monthPart = GetMonthPart(generationDate);
			}

			// Handle the sequence number
			string sequenceNumber = GenerateSequenceNumber(scheme, generationDate, isNewScheme);

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

		/// <summary>
		/// Generates the sequence number part of the invoice number, handling resets as needed.
		/// </summary>
		/// <param name="scheme">The invoice number scheme that contains the current sequence and reset logic.</param>
		/// <param name="generationDate">The generation date used to determine if the sequence should be reset.</param>
		/// <param name="isNewScheme">Determines if a new scheme is being used (resets sequence to 1).</param>
		/// <returns>A string representing the sequence number with proper padding.</returns>
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
	}
}

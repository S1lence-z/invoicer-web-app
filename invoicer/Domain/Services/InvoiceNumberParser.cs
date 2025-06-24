using System.ComponentModel;
using Domain.Interfaces;
using Domain.Models;
using Domain.Utils;
using Shared.Enums;

namespace Domain.Services
{
	public class InvoiceNumberParser : IInvoiceNumberParser
	{
		public bool TryParse(string invoiceNumber, NumberingScheme scheme, out ParsedInvoiceNumberComponents components, out string? errorMessage)
		{
			if (scheme is null || string.IsNullOrEmpty(invoiceNumber))
			{
				errorMessage = "Invoice number or scheme cannot be null or empty.";
				components = default;
				return false;
			}

			if (scheme.SequencePadding < 0)
			{
				errorMessage = "Sequence padding cannot be negative.";
				components = default;
				return false;
			}

			if (scheme.SequencePadding < 1)
			{
				errorMessage = "Sequence padding must be at least 1.";
				components = default;
				return false;
			}

			if (!IsValidAgainstScheme(invoiceNumber, scheme, out components, out errorMessage))
				return false;

			return true;
		}

		private static bool TryExtractPrefix(string invoicenumber, NumberingScheme scheme, out ParsedInvoiceNumberComponents componentsWithPrefix, out string remainingPart, out string? errorMessage)
		{
			componentsWithPrefix = new ParsedInvoiceNumberComponents();
			remainingPart = invoicenumber;
			errorMessage = null;
			if (string.IsNullOrEmpty(scheme.Prefix))
			{
				componentsWithPrefix.Prefix = string.Empty;
				return true;
			}
			if (!remainingPart.StartsWith(scheme.Prefix))
			{
				errorMessage = $"Invoice number does not start with the expected prefix '{scheme.Prefix}'.";
				return false;
			}
			componentsWithPrefix.Prefix = scheme.Prefix;
			remainingPart = remainingPart.Substring(scheme.Prefix.Length);
			return true;
		}

		private static List<(string, int, bool)> GetInvoiceNumberBlueprint(NumberingScheme scheme)
		{
			// Build the expected parts list based on the scheme settings
			var expectedParts = new List<(string type, int fixedLength, bool isNumeric)>();
			int yearLength = scheme.InvoiceNumberYearFormat switch
			{
				YearFormat.None => 0,
				YearFormat.TwoDigit => 2,
				YearFormat.FourDigit => 4,
				_ => throw new ArgumentOutOfRangeException(nameof(scheme.InvoiceNumberYearFormat), "Invalid year format specified in the scheme.")
			};

			// Add parts to the list in the order they are expected in the invoice number
			if (scheme.SequencePosition == Position.Start)
			{
				expectedParts.Add(("sequence", scheme.SequencePadding > 0 ? scheme.SequencePadding : -1, true));
				if (yearLength > 0)
					expectedParts.Add(("year", yearLength, true));
				if (scheme.IncludeMonth)
					expectedParts.Add(("month", 2, true));
			}
			else
			{
				if (yearLength > 0)
					expectedParts.Add(("year", yearLength, true));
				if (scheme.IncludeMonth)
					expectedParts.Add(("month", 2, true));
				expectedParts.Add(("sequence", scheme.SequencePadding > 0 ? scheme.SequencePadding : -1, true));
			}
			return expectedParts;
		}

		private static bool IsValidAgainstScheme(string invoiceNumber, NumberingScheme scheme, out ParsedInvoiceNumberComponents components, out string? errorMessage)
		{
			components = new ParsedInvoiceNumberComponents();
			errorMessage = null;

			bool effectiveUseSeparator = scheme.UseSeperator;
			if (effectiveUseSeparator && string.IsNullOrEmpty(scheme.Seperator))
				effectiveUseSeparator = false;
			string remainingPart = invoiceNumber;

			// Handle and store prefix
			if (!TryExtractPrefix(invoiceNumber, scheme, out components, out remainingPart, out errorMessage))
				return false;

			// Get the expectedParts list
			List<(string, int, bool)> expectedParts = GetInvoiceNumberBlueprint(scheme);

			// Go through the expected parts and validate the remaining part of the invoice number
			bool isFirstPartAfterPrefix = true;
			for (int i = 0; i < expectedParts.Count; i++)
			{
				var (type, fixedLength, isNumeric) = expectedParts[i];

				// If this isn't the first part immediately after the prefix AND the scheme uses separators, then a separator is expected before this current part
				if (!isFirstPartAfterPrefix && effectiveUseSeparator)
				{
					if (string.IsNullOrEmpty(remainingPart) || !remainingPart.StartsWith(scheme.Seperator))
					{
						errorMessage = $"Invoice number does not contain the expected separator '{scheme.Seperator}' before the '{type}' part.";
						return false;
					}
					remainingPart = remainingPart.Substring(scheme.Seperator.Length);
				}
				isFirstPartAfterPrefix = false;

				// Extract and validate the current part
				string currentSegmentValue;
				if (fixedLength > 0)
				{
					if (remainingPart.Length < fixedLength)
					{
						errorMessage = $"Invoice number does not contain enough characters for the '{type}' part. Expected {fixedLength} characters.";
						return false;
					}
					currentSegmentValue = remainingPart.Substring(0, fixedLength);
					remainingPart = remainingPart.Substring(fixedLength);
				}
				else // Part has a variable length (currently only Sequence with Padding == 0, so fixedLength is -1)
				{
					if (string.IsNullOrEmpty(remainingPart))
					{
						errorMessage = $"Invoice number does not contain enough characters for the '{type}' part. Expected at least 1 character.";
						return false;
					}

					int lengthOfNumericSegment = 0;
					while (lengthOfNumericSegment < remainingPart.Length && char.IsDigit(remainingPart[lengthOfNumericSegment]))
						lengthOfNumericSegment++;

					if (lengthOfNumericSegment == 0 && type == "sequence")
					{
						errorMessage = $"Invoice number does not contain a valid sequence number.";
						return false;
					}

					currentSegmentValue = remainingPart.Substring(0, lengthOfNumericSegment);
					remainingPart = remainingPart.Substring(lengthOfNumericSegment);
				}

				if (isNumeric && !currentSegmentValue.All(char.IsDigit))
				{
					errorMessage = $"Invoice number contains non-numeric characters in the '{type}' part.";
					return false;
				}

				// Populate the parsed components
				switch (type)
				{
					case "sequence":
						components.SequenceNumber = currentSegmentValue;
						break;
					case "year":
						components.Year = currentSegmentValue;
						break;
					case "month":
						components.Month = currentSegmentValue;
						break;
					default:
						errorMessage = $"Unknown part type '{type}' in the invoice number.";
						return false;
				}
			}

			if (remainingPart.Length != 0)
			{
				errorMessage = $"Invoice number contains unexpected characters after the expected parts. Remaining part: '{remainingPart}'.";
				return false;
			}

			return true;
		}
	}
}

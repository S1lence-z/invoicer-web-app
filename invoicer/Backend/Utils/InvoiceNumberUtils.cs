using Domain.Enums;
using Domain.Models;

namespace Backend.Utils
{
	public static class InvoiceNumberUtils
	{
		public static string ExtractSequenceNumber(string invoiceNumber, NumberingScheme scheme)
		{
			if (!IsValidAgainstScheme(invoiceNumber, scheme))
				throw new ArgumentException($"Invoice number {invoiceNumber} does not match the numbering scheme.");

			if (scheme == null || string.IsNullOrEmpty(invoiceNumber))
				throw new ArgumentNullException("Invoice number or scheme cannot be null or empty.", nameof(invoiceNumber));

			if (scheme.SequencePadding < 0)
				throw new ArgumentException("Sequence padding cannot be negative.", nameof(scheme.SequencePadding));

			if (invoiceNumber.Length < scheme.Prefix.Length)
				throw new ArgumentException("Invoice number is shorter than the prefix length.", nameof(invoiceNumber));

			return scheme.SequencePosition switch
			{
				Position.Start => ExtractFromStart(invoiceNumber, scheme),
				Position.End => ExtractFromEnd(invoiceNumber, scheme),
				_ => throw new ArgumentException("Invalid sequence position.", nameof(scheme.SequencePosition))
			};
		}


		private static bool IsValidAgainstScheme(string invoiceNumber, NumberingScheme scheme)
		{
			if (string.IsNullOrEmpty(invoiceNumber) || scheme is null)
				return false;

			if (scheme.UseSeperator && string.IsNullOrEmpty(scheme.Seperator))
				scheme.UseSeperator = false;

			string remainingPart = invoiceNumber;
			if (!string.IsNullOrEmpty(scheme.Prefix))
			{
				if (!remainingPart.StartsWith(scheme.Prefix))
					return false;
				remainingPart = remainingPart.Substring(scheme.Prefix.Length);
			}

			var expectedParts = new List<(string type, int fixedLength, bool isNumeric)>();
			int yearLength = scheme.InvoiceNumberYearFormat switch
			{
				YearFormat.None => 0,
				YearFormat.TwoDigit => 2,
				YearFormat.FourDigit => 4,
				_ => throw new ArgumentOutOfRangeException(nameof(scheme.InvoiceNumberYearFormat), "Invalid year format specified in the scheme.")
			};

			// Add parts to the list in the order they are expected in the invoice number.
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

			bool isFirstPartAfterPrefix = true;
			for (int i = 0; i < expectedParts.Count; i++)
			{
				var (type, fixedLength, isNumeric) = expectedParts[i];

				// If this isn't the first part immediately after the prefix AND the scheme uses separators, then a separator is expected before this current part.
				if (!isFirstPartAfterPrefix && scheme.UseSeperator)
				{
					if (string.IsNullOrEmpty(remainingPart) || !remainingPart.StartsWith(scheme.Seperator))
						return false;
					remainingPart = remainingPart.Substring(scheme.Seperator.Length);
				}
				isFirstPartAfterPrefix = false;

				// 5b. Part Extraction and Validation:
				string currentSegmentValue;
				if (fixedLength > 0) // Part has a defined, fixed length (e.g., Year, Month, Sequence with Padding > 0).
				{
					if (remainingPart.Length < fixedLength)
						return false;
					currentSegmentValue = remainingPart.Substring(0, fixedLength);
					remainingPart = remainingPart.Substring(fixedLength);
				}
				else // Part has a variable length (currently only Sequence with Padding == 0, so fixedLength is -1).
				{
					if (string.IsNullOrEmpty(remainingPart))
						return false;

					int lengthOfNumericSegment = 0;
					while (lengthOfNumericSegment < remainingPart.Length && char.IsDigit(remainingPart[lengthOfNumericSegment]))
						lengthOfNumericSegment++;

					if (lengthOfNumericSegment == 0)
						return false;

					currentSegmentValue = remainingPart.Substring(0, lengthOfNumericSegment);
					remainingPart = remainingPart.Substring(lengthOfNumericSegment);
				}

				if (isNumeric && !currentSegmentValue.All(char.IsDigit))
					return false;
			}
			return remainingPart.Length == 0;
		}

		private static string ExtractFromStart(string invoiceNumber, NumberingScheme scheme)
		{
			if (string.IsNullOrEmpty(invoiceNumber))
				throw new ArgumentNullException("Invoice number cannot be null or empty.", nameof(invoiceNumber));

			if (invoiceNumber.Length < scheme.Prefix.Length)
				throw new ArgumentException("Invoice number is shorter than the prefix length.", nameof(invoiceNumber));

			string contentAfterPrefix = invoiceNumber.Substring(scheme.Prefix.Length);
			string sequenceCandidate;

			if (scheme.UseSeperator && !string.IsNullOrEmpty(scheme.Seperator))
			{
				int separatorIndex = contentAfterPrefix.IndexOf(scheme.Seperator);
				if (separatorIndex != -1)
					sequenceCandidate = contentAfterPrefix.Substring(0, separatorIndex);
				else
					throw new ArgumentException("Separator not found in the invoice number.", nameof(invoiceNumber));
			}
			else
			{
				// This is very unlikely to happen, but if the scheme doesn't use a separator, we need to extract the sequence from the start of the string
				if (scheme.SequencePadding == 0)
					throw new ArgumentException("The schemes sequence padding is zero. It must be at least one");
				else
					sequenceCandidate = contentAfterPrefix.Substring(0, Math.Min(contentAfterPrefix.Length, scheme.SequencePadding));
			}

			sequenceCandidate = sequenceCandidate.Trim();

			if (scheme.SequencePadding > 0)
			{
				if (sequenceCandidate.Length > scheme.SequencePadding)
					return sequenceCandidate.Substring(0, scheme.SequencePadding);
				else
					return sequenceCandidate.PadLeft(scheme.SequencePadding, '0');
			}
			return sequenceCandidate;
		}

		private static string ExtractFromEnd(string invoiceNumber, NumberingScheme scheme)
		{
			if (string.IsNullOrEmpty(invoiceNumber))
				throw new ArgumentNullException("Invoice number cannot be null or empty.", nameof(invoiceNumber));


			string sequenceCandidate;
			if (scheme.SequencePadding > 0)
			{
				if (invoiceNumber.Length < scheme.SequencePadding)
					throw new FormatException($"Invoice number length ({invoiceNumber.Length}) is less than the specified sequence padding ({scheme.SequencePadding}).");
				sequenceCandidate = invoiceNumber.Substring(invoiceNumber.Length - scheme.SequencePadding);
			}
			else
				throw new ArgumentException("The schemes sequence padding is zero. It must be at least one");
			return sequenceCandidate.Trim();
		}
	}
}
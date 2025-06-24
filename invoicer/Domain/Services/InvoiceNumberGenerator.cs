using Domain.Models;
using Domain.Interfaces;
using Shared.Enums;

namespace Domain.Services
{
	public sealed class InvoiceNumberGenerator(IInvoiceNumberParser invoiceNumberParser) : IInvoiceNumberGenerator
	{
		public InvoiceNumber Generate(NumberingScheme scheme, EntityInvoiceNumberingSchemeState currentState, DateTime generationDate)
		{
			int sequenceNumber = ShouldResetSequenceNumber(scheme, currentState, generationDate) ? 1 : currentState.LastSequenceNumber + 1;

			// Format the sequence number with padding
			string formattedSequenceNumber = scheme.SequencePadding > 0
				? sequenceNumber.ToString($"D{scheme.SequencePadding}")
				: sequenceNumber.ToString();

			string yearPart = GetYearPart(generationDate, scheme.InvoiceNumberYearFormat);
			string monthPart = scheme.IncludeMonth ? GetMonthPart(generationDate) : string.Empty;
			string separator = scheme.UseSeperator ? scheme.Seperator : string.Empty;

			List<string> parts = ConstructInvoiceNumberParts(scheme, formattedSequenceNumber, yearPart, monthPart);

			string invoiceNumber = scheme.Prefix + string.Join(separator, parts.Where(p => !string.IsNullOrEmpty(p)));
			return InvoiceNumber.FromString(invoiceNumber, scheme, invoiceNumberParser);
		}

		private static List<string> ConstructInvoiceNumberParts(NumberingScheme scheme, string formattedSequenceNumber, string yearPart, string monthPart)
		{
			List<string> parts = [];
			switch (scheme.SequencePosition)
			{
				case Position.Start:
					parts.Add(formattedSequenceNumber);
					if (scheme.InvoiceNumberYearFormat != YearFormat.None)
						parts.Add(yearPart);
					if (scheme.IncludeMonth)
						parts.Add(monthPart);
					break;
				case Position.End:
					if (scheme.InvoiceNumberYearFormat != YearFormat.None)
						parts.Add(yearPart);
					if (scheme.IncludeMonth)
						parts.Add(monthPart);
					parts.Add(formattedSequenceNumber);
					break;
				default:
					throw new InvalidOperationException($"Invalid sequence position: {scheme.SequencePosition}");
			}
			return parts;
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

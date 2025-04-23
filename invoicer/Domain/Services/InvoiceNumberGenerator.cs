using Domain.Models;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Services
{
	public sealed class InvoiceNumberGenerator : IInvoiceNumberGenerator
	{
		public string GenerateInvoiceNumber(NumberingScheme scheme, EntityInvoiceNumberingSchemeState state, DateTime generationDate)
		{
			int sequenceNumber = ShouldResetSequenceNumber(scheme, state, generationDate) ? 1 : state.LastSequenceNumber + 1;

			// Format the sequence number with padding
			string formattedSequenceNumber = scheme.SequencePadding > 0
				? sequenceNumber.ToString($"D{scheme.SequencePadding}")
				: sequenceNumber.ToString();

			string yearPart = GetYearPart(generationDate, scheme.InvoiceNumberYearFormat);
			string monthPart = scheme.IncludeMonth ? GetMonthPart(generationDate) : string.Empty;
			string separator = scheme.UseSeperator ? scheme.Seperator : string.Empty;

			List<string> parts = [];

			if (scheme.SequencePosition == Position.Start)
			{
				parts.Add(formattedSequenceNumber);
				if (scheme.InvoiceNumberYearFormat != YearFormat.None)
					parts.Add(yearPart);
				if (scheme.IncludeMonth)
					parts.Add(monthPart);
			}
			else if (scheme.SequencePosition == Position.End)
			{
				if (scheme.InvoiceNumberYearFormat != YearFormat.None)
					parts.Add(yearPart);
				if (scheme.IncludeMonth)
					parts.Add(monthPart);
				parts.Add(formattedSequenceNumber);
			}
			else
			{
				throw new InvalidOperationException($"Invalid sequence position: {scheme.SequencePosition}");
			}

			return scheme.Prefix + string.Join(separator, parts.Where(p => !string.IsNullOrEmpty(p)));
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

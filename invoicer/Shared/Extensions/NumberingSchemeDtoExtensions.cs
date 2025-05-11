using Shared.DTOs;
using Shared.Enums;

namespace Shared.Extensions
{
	public static class NumberingSchemeDtoExtensions
	{
		private static int GetLargestNumber(int digitCount)
		{
			string biggestDigitString = new('9', digitCount);
			return int.Parse(biggestDigitString);
		}

		public static string ShowNumberingSchemePreview(this NumberingSchemeDto numberingScheme)
		{
			int previewNumbers = 2;
			List<string> previewNumbersList = [];
			for (int i = 1; i <= previewNumbers; i++)
			{
				var previewNumber = numberingScheme.GetPreview(i);
				previewNumbersList.Add(previewNumber);
			}
			previewNumbersList.Add("...");
			int lastSequenceNumber = GetLargestNumber(numberingScheme.SequencePadding);
			string lastNumber = numberingScheme.GetPreview(lastSequenceNumber);
			previewNumbersList.Add(lastNumber);
			return string.Join(", ", previewNumbersList);
		}

		public static string GetPreview(this NumberingSchemeDto numberingScheme, int sequenceNumber)
		{
			string yearFormat = numberingScheme.YearFormat switch
			{
				YearFormat.FourDigit => DateTime.Now.ToString("yyyy"),
				YearFormat.TwoDigit => DateTime.Now.ToString("yy"),
				_ => string.Empty
			};

			string separator = numberingScheme.UseSeperator ? numberingScheme.Seperator : string.Empty;
			string monthFormat = numberingScheme.IncludeMonth ? DateTime.Now.ToString("MM") : string.Empty;
			string formattedSequenceNumber = numberingScheme.SequencePadding > 0
				? sequenceNumber.ToString($"D{numberingScheme.SequencePadding}")
				: sequenceNumber.ToString();

			List<string> parts = [];

			if (numberingScheme.SequencePosition == Position.Start)
			{
				parts.Add(formattedSequenceNumber);
				if (numberingScheme.YearFormat != YearFormat.None)
					parts.Add(yearFormat);
				if (numberingScheme.IncludeMonth)
					parts.Add(monthFormat);
			}
			else if (numberingScheme.SequencePosition == Position.End)
			{
				if (numberingScheme.YearFormat != YearFormat.None)
					parts.Add(yearFormat);
				if (numberingScheme.IncludeMonth)
					parts.Add(monthFormat);
				parts.Add(formattedSequenceNumber);
			}
			else
			{
				throw new InvalidOperationException($"Invalid sequence position: {numberingScheme.SequencePosition}");
			}
			return numberingScheme.Prefix + string.Join(separator, parts.Where(p => !string.IsNullOrEmpty(p)));
		}
	}
}

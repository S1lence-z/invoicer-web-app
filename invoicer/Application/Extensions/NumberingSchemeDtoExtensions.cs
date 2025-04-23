using Application.DTOs;
using Domain.Enums;

namespace Application.Extensions
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
			var previewNumbersList = new List<string>();
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
			var yearFormat = numberingScheme.YearFormat switch
			{
				YearFormat.FourDigit => DateTime.Now.ToString("yyyy"),
				YearFormat.TwoDigit => DateTime.Now.ToString("yy"),
				_ => string.Empty
			};

			var seperator = numberingScheme.UseSeperator ? numberingScheme.Seperator : string.Empty;

			var monthFormat = numberingScheme.IncludeMonth ? DateTime.Now.ToString("MM") : string.Empty;

			var sequenceFormat = numberingScheme.SequencePadding > 0
				? sequenceNumber.ToString($"D{numberingScheme.SequencePadding}")
			: sequenceNumber.ToString();

			return numberingScheme.SequencePosition switch
			{
				Position.Start => $"{numberingScheme.Prefix}{sequenceFormat}{seperator}{yearFormat}{seperator}{monthFormat}",
				Position.End => $"{numberingScheme.Prefix}{yearFormat}{seperator}{monthFormat}{seperator}{sequenceFormat}",
				_ => throw new InvalidOperationException("Invalid sequence position.")
			};
		}
	}
}

using Domain.Models;
using Domain.Enums;

namespace Backend.Utils
{
	public static class EntityInvoiceNumberingStateUpdater
	{
		public static void SetNewSequenceNumber(EntityInvoiceNumberingSchemeState state, int newSequenceNumber)
		{
			state.LastSequenceNumber = newSequenceNumber;
			int currentYear = DateTime.Now.Year;
			int currentMonth = DateTime.Now.Month;
			if (currentYear != state.LastGenerationYear) state.LastGenerationYear = currentYear;
			if (currentMonth != state.LastGenerationMonth) state.LastGenerationMonth = currentMonth;
		}

		public static string ExtractSequenceNumber(string invoiceNumber, NumberingScheme scheme)
		{
			string sequencePart = invoiceNumber;
			int separatorIndex = invoiceNumber.IndexOf(scheme.Seperator);
			switch (scheme.SequencePosition)
			{
				case Position.Start:
					sequencePart = invoiceNumber.Substring(scheme.Prefix.Length);
					if (scheme.UseSeperator)
					{
						string[] splitNumber = sequencePart.Split(scheme.Seperator);
						sequencePart = splitNumber[0].Trim();
					}

					break;

				case Position.End:
					separatorIndex = invoiceNumber.LastIndexOf(scheme.Seperator);
					if (separatorIndex != -1)
						sequencePart = invoiceNumber.Substring(separatorIndex + scheme.Seperator.Length);
					break;

				default:
					throw new InvalidOperationException($"Invalid sequence position: {scheme.SequencePosition}");
			}
			return sequencePart;
		}

		public static bool IsValidAgainstScheme(string invoiceNumber, NumberingScheme scheme)
		{
			string sequencePart = ExtractSequenceNumber(invoiceNumber, scheme);
			if (sequencePart.Length > scheme.SequencePadding)
				return false;
			if (scheme.UseSeperator)
			{
				int separatorIndex = invoiceNumber.IndexOf(scheme.Seperator);
				if (separatorIndex == -1 || separatorIndex != scheme.Prefix.Length + scheme.SequencePadding)
					return false;
			}
			return true;
		}
	}
}

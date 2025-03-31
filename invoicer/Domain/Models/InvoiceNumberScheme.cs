using Domain.Enums;

namespace Domain.Models
{
	public class InvoiceNumberScheme
	{
		public int Id { get; set; }

		// Entity ID
		public int EntityId { get; set; }
		public virtual Entity? Entity { get; set; }

		// Prefix
		public string Prefix { get; set; } = string.Empty;
		
		// Seperator
		public bool UseSeperator { get; set; } = true;
		public string Seperator { get; set; } = "-";

		// Sequence number
		public InvoiceNumberSequencePosition SequencePosition { get; set; } = InvoiceNumberSequencePosition.Start;
		public int SequencePadding { get; set; } = 3;

		// Year and month
		public InvoiceNumberYearFormat InvoiceNumberYearFormat { get; set; } = InvoiceNumberYearFormat.None;
		public bool IncludeMonth { get; set; } = true;
		public InvoiceNumberResetFrequency ResetFrequency { get; set; } = InvoiceNumberResetFrequency.Yearly;

		// Current state
		public int LastSequenceNumber { get; set; } = 0;
		public int LastGenerationYear { get; set; } = 0;
		public int LastGenerationMonth { get; set; } = 0;

		// Define default numbering scheme
		public static InvoiceNumberScheme CreateDefault(int entityId)
		{
			// Output will be: FAK001-2021-01
			return new InvoiceNumberScheme
			{
				EntityId = entityId,
				Prefix = "FAK",
				UseSeperator = true,
				Seperator = "-",
				SequencePosition = InvoiceNumberSequencePosition.Start,
				SequencePadding = 3,
				InvoiceNumberYearFormat = InvoiceNumberYearFormat.FourDigit,
				IncludeMonth = true,
				LastSequenceNumber = 0,
				LastGenerationYear = 0,
				LastGenerationMonth = 0
			};
		}
	}
}

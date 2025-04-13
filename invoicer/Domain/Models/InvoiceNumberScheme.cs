using Domain.Enums;

namespace Domain.Models
{
	public class InvoiceNumberScheme
	{
		public int Id { get; set; }

		// Settings
		public string Prefix { get; set; } = string.Empty;
		public bool UseSeperator { get; set; } = true;
		public string Seperator { get; set; } = "-";
		public InvoiceNumberSequencePosition SequencePosition { get; set; } = InvoiceNumberSequencePosition.Start;
		public int SequencePadding { get; set; } = 3;
		public InvoiceNumberYearFormat InvoiceNumberYearFormat { get; set; } = InvoiceNumberYearFormat.FourDigit;
		public bool IncludeMonth { get; set; } = true;
		public InvoiceNumberResetFrequency ResetFrequency { get; set; } = InvoiceNumberResetFrequency.Yearly;
		public bool IsDefault { get; init; } = false;

		// Navigation properties
		public virtual ICollection<Entity> EntitiesUsingScheme { get; set; } = [];
		public virtual ICollection<Invoice> InvoicesGeneratedWithScheme { get; set; } = [];
		public virtual ICollection<EntityInvoiceNumberSchemeState> EntityInvoiceNumberSchemeStates { get; set; } = [];

		// Define default numbering scheme
		public static InvoiceNumberScheme CreateDefault()
		{
			return new InvoiceNumberScheme
			{
				Prefix = "INV",
				UseSeperator = true,
				Seperator = "-",
				SequencePosition = InvoiceNumberSequencePosition.Start,
				SequencePadding = 3,
				InvoiceNumberYearFormat = InvoiceNumberYearFormat.FourDigit,
				IncludeMonth = true,
				IsDefault = true,
			};
		}
	}
}

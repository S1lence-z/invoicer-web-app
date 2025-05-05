using Domain.Enums;

namespace Domain.Models
{
	public class NumberingScheme
	{
		public int Id { get; set; }

		// Settings
		public string Prefix { get; set; } = string.Empty;
		public bool UseSeperator { get; set; } = true;
		public string Seperator { get; set; } = "-";
		public Position SequencePosition { get; set; } = Position.Start;
		public int SequencePadding { get; set; } = 3;
		public YearFormat InvoiceNumberYearFormat { get; set; } = YearFormat.FourDigit;
		public bool IncludeMonth { get; set; } = true;
		public ResetFrequency ResetFrequency { get; set; } = ResetFrequency.Yearly;
		public bool IsDefault { get; set; } = false;

		// Navigation properties
		public virtual ICollection<Entity> EntitiesUsingScheme { get; set; } = [];
		public virtual ICollection<Invoice> InvoicesGeneratedWithScheme { get; set; } = [];
		public virtual ICollection<EntityInvoiceNumberingSchemeState> EntityNumberingSchemeState { get; set; } = [];

		// Define default numbering scheme
		public static NumberingScheme CreateDefault()
		{
			return new NumberingScheme
			{
				Id = 1,
				Prefix = "INV",
				UseSeperator = true,
				Seperator = "-",
				SequencePosition = Position.Start,
				SequencePadding = 3,
				InvoiceNumberYearFormat = YearFormat.FourDigit,
				IncludeMonth = true,
				IsDefault = true,
			};
		}
	}
}

namespace Domain.Models
{
	public class EntityInvoiceNumberingSchemeState
	{
		// Primary key
		public int EntityId { get; set; }

		// Navigation properties
		public virtual Entity Entity { get; set; } = null!;
		public virtual NumberingScheme InvoiceNumberingScheme { get; set; } = null!;

		// Current state
		public int LastSequenceNumber { get; set; } = 0;
		public int LastGenerationYear { get; set; } = 0;
		public int LastGenerationMonth { get; set; } = 0;

		public EntityInvoiceNumberingSchemeState UpdateForNext()
		{
			LastSequenceNumber++;
			int currentYear = DateTime.Now.Year;
			int currentMonth = DateTime.Now.Month;
			if (currentYear != LastGenerationYear) LastGenerationYear = currentYear;
			if (currentMonth != LastGenerationMonth) LastGenerationMonth = currentMonth;
			return this;
		}
	}
}
